import React, { useState, useEffect, useCallback } from 'react';
import { Box, Typography, Paper, Grid, TextField, Button, MenuItem, Autocomplete, CircularProgress, Alert, ToggleButton, ToggleButtonGroup, Card, CardContent } from '@mui/material';
import { useUser } from '../context/UserContext';
import axiosAPI from '../variables/axiosAPI';

const AddEmployeePage = () => {
    const { user } = useUser();
    const [mode, setMode] = useState('new'); // 'new' or 'edit'
    const [lookups, setLookups] = useState({ departments: [], execHeads: [], subAreas: [], designations: [] });
    const [formData, setFormData] = useState({
        perno: '', employeeName: '', email: '', contactNo: '', joiningDate: '',
        deptCode: '', execHeadCode: '', subAreaCode: '', designationCode: '',
        eqvLevel: '', sGrade: '', empClass: '', reportingTo: '', buhrNo: '', buhrName: '',
        dottedPno: '', persExecPno: '', year: user.fiscalYear, cycle: user.cycle
    });
    const [searchQuery, setSearchQuery] = useState('');
    const [searchResults, setSearchResults] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);

    const fetchLookups = useCallback(async () => {
        try {
            const response = await axiosAPI.get(`/employee/lookups?adminId=${user.userId}&isSuperAdmin=${user.isSuperAdmin}`);
            setLookups(response.data);
        } catch (err) {
            setError('Failed to fetch lookup data');
        }
    }, [user.userId, user.isSuperAdmin]);

    useEffect(() => {
        fetchLookups();
    }, [fetchLookups]);

    useEffect(() => {
        const delayDebounceFn = setTimeout(() => {
            if (searchQuery.length > 2) {
                axiosAPI.get(`/employee/search?prefix=${searchQuery}`).then((res) => setSearchResults(res.data));
            }
        }, 300);
        return () => clearTimeout(delayDebounceFn);
    }, [searchQuery]);

    const handleSearchSelect = async (event, value) => {
        if (!value) return;
        const perno = value.split('(')[1].replace(')', '');
        try {
            setIsLoading(true);
            const response = await axiosAPI.get(`/employee/details?perno=${perno}&year=${user.fiscalYear}&cycle=${user.cycle}`);
            setFormData(response.data);
            setMode('edit');
        } catch (err) {
            setError('Failed to fetch employee details');
        } finally {
            setIsLoading(false);
        }
    };

    const handleSave = async () => {
        try {
            setIsLoading(true);
            const endpoint = mode === 'new' ? '/employee/save' : '/employee/update';
            await axiosAPI.post(endpoint, { ...formData, year: user.fiscalYear, cycle: user.cycle });
            alert(`Employee details ${mode === 'new' ? 'saved' : 'updated'} successfully`);
            if (mode === 'new') setFormData({ ...formData, perno: '', employeeName: '' });
        } catch (err) {
            setError(`Failed to ${mode === 'new' ? 'save' : 'update'} employee data`);
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <Box sx={{ py: 4 }}>
            <Typography variant="h4" gutterBottom color="primary">Employee Management</Typography>

            <ToggleButtonGroup value={mode} exclusive onChange={(e, next) => setMode(next || mode)} sx={{ mb: 3 }}>
                <ToggleButton value="new">Add New Employee</ToggleButton>
                <ToggleButton value="edit">Edit Employee</ToggleButton>
            </ToggleButtonGroup>

            {mode === 'edit' && (
                <Paper sx={{ p: 2, mb: 3 }}>
                    <Autocomplete
                        options={searchResults}
                        onInputChange={(e, val) => setSearchQuery(val)}
                        onChange={handleSearchSelect}
                        renderInput={(params) => <TextField {...params} label="Search Employee (Name or PNo)" fullWidth />}
                    />
                </Paper>
            )}

            <Card>
                <CardContent>
                    <Grid container spacing={2}>
                        <Grid item xs={3}>
                            <TextField label="PNo" fullWidth value={formData.perno} onChange={(e) => setFormData({ ...formData, perno: e.target.value })} disabled={mode === 'edit'} />
                        </Grid>
                        <Grid item xs={6}>
                            <TextField label="Employee Name" fullWidth value={formData.employeeName} onChange={(e) => setFormData({ ...formData, employeeName: e.target.value })} />
                        </Grid>
                        <Grid item xs={3}>
                            <TextField label="Email" fullWidth value={formData.email} onChange={(e) => setFormData({ ...formData, email: e.target.value })} />
                        </Grid>

                        <Grid item xs={3}>
                            <TextField select label="Department" fullWidth value={formData.deptCode} onChange={(e) => setFormData({ ...formData, deptCode: e.target.value })}>
                                {lookups.departments.map((opt) => (
                                    <MenuItem key={opt.Id} value={opt.Id}>{opt.Text}</MenuItem>
                                ))}
                            </TextField>
                        </Grid>
                        <Grid item xs={3}>
                            <TextField select label="Designation" fullWidth value={formData.designationCode} onChange={(e) => setFormData({ ...formData, designationCode: e.target.value })}>
                                {lookups.designations.map((opt) => (
                                    <MenuItem key={opt.Id} value={opt.Id}>{opt.Text}</MenuItem>
                                ))}
                            </TextField>
                        </Grid>
                        <Grid item xs={3}>
                            <TextField select label="Exec Head" fullWidth value={formData.execHeadCode} onChange={(e) => setFormData({ ...formData, execHeadCode: e.target.value })}>
                                {lookups.execHeads.map((opt) => (
                                    <MenuItem key={opt.Id} value={opt.Id}>{opt.Text}</MenuItem>
                                ))}
                            </TextField>
                        </Grid>
                        <Grid item xs={3}>
                            <TextField select label="Sub Area" fullWidth value={formData.subAreaCode} onChange={(e) => setFormData({ ...formData, subAreaCode: e.target.value })}>
                                {lookups.subAreas.map((opt) => (
                                    <MenuItem key={opt.Id} value={opt.Id}>{opt.Text}</MenuItem>
                                ))}
                            </TextField>
                        </Grid>

                        <Grid item xs={3}><TextField label="Eqv Level" fullWidth value={formData.eqvLevel} onChange={(e) => setFormData({ ...formData, eqvLevel: e.target.value })} /></Grid>
                        <Grid item xs={3}><TextField label="SGrade" fullWidth value={formData.sGrade} onChange={(e) => setFormData({ ...formData, sGrade: e.target.value })} /></Grid>
                        <Grid item xs={3}><TextField label="Emp Class" fullWidth value={formData.empClass} onChange={(e) => setFormData({ ...formData, empClass: e.target.value })} /></Grid>
                        <Grid item xs={3}><TextField label="Joining Date" fullWidth value={formData.joiningDate} onChange={(e) => setFormData({ ...formData, joiningDate: e.target.value })} placeholder="dd-mm-yyyy" /></Grid>

                        <Grid item xs={3}><TextField label="Reporting To" fullWidth value={formData.reportingTo} onChange={(e) => setFormData({ ...formData, reportingTo: e.target.value })} /></Grid>
                        <Grid item xs={3}><TextField label="Dotted PNo" fullWidth value={formData.dottedPno} onChange={(e) => setFormData({ ...formData, dottedPno: e.target.value })} /></Grid>
                        <Grid item xs={3}><TextField label="Pers Exec PNo" fullWidth value={formData.persExecPno} onChange={(e) => setFormData({ ...formData, persExecPno: e.target.value })} /></Grid>
                        <Grid item xs={3}><TextField label="Contact No" fullWidth value={formData.contactNo} onChange={(e) => setFormData({ ...formData, contactNo: e.target.value })} /></Grid>

                        <Grid item xs={4}><TextField label="Step 1 Start" fullWidth value={formData.step1Start} onChange={(e) => setFormData({ ...formData, step1Start: e.target.value })} placeholder="dd-mm-yyyy" /></Grid>
                        <Grid item xs={4}><TextField label="Step 2 Start" fullWidth value={formData.step2Start} onChange={(e) => setFormData({ ...formData, step2Start: e.target.value })} placeholder="dd-mm-yyyy" /></Grid>
                        <Grid item xs={4}><TextField label="Step 3 Start" fullWidth value={formData.step3Start} onChange={(e) => setFormData({ ...formData, step3Start: e.target.value })} placeholder="dd-mm-yyyy" /></Grid>

                        <Grid item xs={4}><TextField label="Step 1 End" fullWidth value={formData.step1End} onChange={(e) => setFormData({ ...formData, step1End: e.target.value })} placeholder="dd-mm-yyyy" /></Grid>
                        <Grid item xs={4}><TextField label="Step 2 End" fullWidth value={formData.step2End} onChange={(e) => setFormData({ ...formData, step2End: e.target.value })} placeholder="dd-mm-yyyy" /></Grid>
                        <Grid item xs={4}><TextField label="Step 3 End" fullWidth value={formData.step3End} onChange={(e) => setFormData({ ...formData, step3End: e.target.value })} placeholder="dd-mm-yyyy" /></Grid>

                        <Grid item xs={12}>
                            <Box sx={{ display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
                                <Button variant="contained" color="primary" onClick={handleSave} disabled={isLoading}>
                                    {mode === 'new' ? 'Save Employee' : 'Update Employee'}
                                </Button>
                                <Button variant="outlined" onClick={() => setFormData({})}>Clear</Button>
                            </Box>
                        </Grid>
                    </Grid>
                </CardContent>
            </Card>

            {isLoading && <CircularProgress sx={{ mt: 2 }} />}
            {error && <Alert severity="error" sx={{ mt: 2 }}>{error}</Alert>}
        </Box>
    );
};

export default AddEmployeePage;
