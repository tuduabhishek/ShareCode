import React, { useState, useEffect, useCallback } from 'react';
import { Box, Typography, Paper, Grid, Button, TextField, MenuItem, Select, FormControl, InputLabel, Card, CardContent, Divider, Alert, Autocomplete } from '@mui/material';
import { useUser } from '../context/UserContext';
import axiosAPI from '../variables/axiosAPI';
import { useParams, useNavigate } from 'react-router-dom';
import { ReactTabulator } from 'react-tabulator';

const ManageRespondentsPage = () => {
    const { perno } = useParams();
    const { user } = useUser();
    const navigate = useNavigate();
    const [assessee, setAssessee] = useState(null);
    const [respondents, setRespondents] = useState([]);
    const [rules, setRules] = useState([]);
    const [loading, setLoading] = useState(false);
    const [searchQuery, setSearchQuery] = useState('');
    const [searchResults, setSearchResults] = useState([]);
    const [selectedEmp, setSelectedEmp] = useState(null);
    const [category, setCategory] = useState('');
    const [externalForm, setExternalForm] = useState({
        name: '',
        email: '',
        designation: '',
        department: '',
        category: ''
    });

    const refreshData = useCallback(async () => {
        try {
            setLoading(true);
            const [listRes, assesRes] = await Promise.all([
                axiosAPI.get('/respondent/list', { params: { assesPno: perno, fy: user.fiscalYear, cycle: user.cycle } }),
                axiosAPI.get('/employee/search', { params: { query: perno } }) // reusing employee search to get assessee details
            ]);
            setRespondents(listRes.data);
            if (assesRes.data.length > 0) {
                setAssessee(assesRes.data[0]);
                const rulesRes = await axiosAPI.get('/respondent/rules', { params: { level: assesRes.data[0].Level || 'I4' } });
                setRules(rulesRes.data);
            }
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    }, [perno, user]);

    useEffect(() => { refreshData(); }, [refreshData]);

    const handleSearch = async (val) => {
        setSearchQuery(val);
        if (val.length > 2) {
            const res = await axiosAPI.get('/employee/search', { params: { query: val } });
            setSearchResults(res.data);
        }
    };

    const addInternal = async () => {
        if (!selectedEmp || !category) return;
        try {
            await axiosAPI.post('/respondent/add', {
                AssesPno: perno,
                AssesLevel: assessee.Level,
                Perno: selectedEmp.Perno,
                Name: selectedEmp.Name,
                Designation: selectedEmp.Designation,
                Department: selectedEmp.Department,
                Email: selectedEmp.Email,
                Category: category,
                Type: 'ORG',
                AdminId: user.userId,
                FiscalYear: user.fiscalYear,
                Cycle: user.cycle
            });
            setSelectedEmp(null);
            setCategory('');
            refreshData();
        } catch (err) {
            alert(err.response?.data?.Message || "Failed to add respondent");
        }
    };

    const addExternal = async () => {
        try {
            await axiosAPI.post('/respondent/add', {
                ...externalForm,
                AssesPno: perno,
                AssesLevel: assessee.Level,
                Type: 'NORG',
                AdminId: user.userId,
                FiscalYear: user.fiscalYear,
                Cycle: user.cycle
            });
            setExternalForm({ name: '', email: '', designation: '', department: '', category: '' });
            refreshData();
        } catch (err) {
            alert("Failed to add external respondent");
        }
    };

    const removeRespondent = async (row) => {
        if (!window.confirm("Remove this respondent?")) return;
        try {
            await axiosAPI.delete('/respondent/remove', {
                params: {
                    assesPno: perno,
                    respPno: row.Perno,
                    category: row.Category,
                    fy: user.fiscalYear,
                    cycle: user.cycle
                }
            });
            refreshData();
        } catch (err) {
            alert("Failed to remove");
        }
    };

    const submitList = async () => {
        try {
            await axiosAPI.post('/respondent/submit', null, {
                params: { assesPno: perno, fy: user.fiscalYear, cycle: user.cycle, me: user.userId }
            });
            alert("List submitted successfully");
            navigate('/respondents');
        } catch (err) {
            alert("Submission failed");
        }
    };

    const columns = [
        { title: "Category", field: "Category", width: 150 },
        { title: "Perno", field: "Perno", width: 100 },
        { title: "Name", field: "Name", width: 200 },
        { title: "Email", field: "Email", width: 200 },
        {
            title: "Actions",
            formatter: () => '<button style="color: red; cursor: pointer;">Delete</button>',
            cellClick: (e, cell) => removeRespondent(cell.getData())
        }
    ];

    return (
        <Box sx={{ p: 3 }}>
            <Typography variant="h4">Manage Respondents for {assessee?.Name} ({perno})</Typography>
            <Divider sx={{ my: 2 }} />

            <Grid container spacing={3}>
                <Grid item xs={12} md={6}>
                    <Card variant="outlined">
                        <CardContent>
                            <Typography variant="h6">Add Internal Respondent</Typography>
                            <Autocomplete
                                options={searchResults}
                                getOptionLabel={(option) => `${option.Name} (${option.Perno})`}
                                onInputChange={(e, val) => handleSearch(val)}
                                onChange={(e, val) => setSelectedEmp(val)}
                                renderInput={(params) => <TextField {...params} label="Search Employee" size="small" margin="normal" />}
                            />
                            <FormControl fullWidth size="small" margin="normal">
                                <InputLabel>Category</InputLabel>
                                <Select value={category} onChange={(e) => setCategory(e.target.value)}>
                                    {rules.map(r => <MenuItem key={r.Category} value={r.Category}>{r.CategoryName}</MenuItem>)}
                                </Select>
                            </FormControl>
                            <Button variant="contained" onClick={addInternal} sx={{ mt: 1 }}>Add</Button>
                        </CardContent>
                    </Card>
                </Grid>

                <Grid item xs={12} md={6}>
                    <Card variant="outlined">
                        <CardContent>
                            <Typography variant="h6">Add External Respondent</Typography>
                            <TextField fullWidth size="small" label="Name" margin="dense" value={externalForm.name} onChange={(e) => setExternalForm({ ...externalForm, name: e.target.value })} />
                            <TextField fullWidth size="small" label="Email" margin="dense" value={externalForm.email} onChange={(e) => setExternalForm({ ...externalForm, email: e.target.value })} />
                            <FormControl fullWidth size="small" margin="dense">
                                <InputLabel>Category</InputLabel>
                                <Select value={externalForm.category} onChange={(e) => setExternalForm({ ...externalForm, category: e.target.value })}>
                                    {rules.map(r => <MenuItem key={r.Category} value={r.Category}>{r.CategoryName}</MenuItem>)}
                                </Select>
                            </FormControl>
                            <Button variant="contained" onClick={addExternal} sx={{ mt: 1 }}>Add External</Button>
                        </CardContent>
                    </Card>
                </Grid>

                <Grid item xs={12}>
                    <Typography variant="h6" sx={{ mt: 2 }}>Current Respondents</Typography>
                    <Paper sx={{ p: 1, mt: 1 }}>
                        <ReactTabulator data={respondents} columns={columns} layout="fitColumns" />
                    </Paper>
                </Grid>

                <Grid item xs={12} sx={{ textAlign: 'right', mt: 2 }}>
                    <Button variant="contained" color="success" size="large" onClick={submitList}>
                        Submit Respondent List
                    </Button>
                </Grid>
            </Grid>
        </Box>
    );
};

export default ManageRespondentsPage;
