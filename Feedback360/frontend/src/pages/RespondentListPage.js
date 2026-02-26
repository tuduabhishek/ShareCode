import React, { useState, useEffect, useCallback } from 'react';
import { Box, Typography, Paper, Grid, Button, TextField, MenuItem, Select, FormControl, InputLabel, Card, CardContent, Chip, IconButton, Tooltip } from '@mui/material';
import { PlayArrow, ManageAccounts, CheckCircle, Cancel } from '@mui/icons-material';
import { useUser } from '../context/UserContext';
import axiosAPI from '../variables/axiosAPI';
import { ReactTabulator } from 'react-tabulator';
import 'react-tabulator/lib/styles.css';
import 'react-tabulator/lib/css/tabulator_materialize.min.css';
import { useNavigate } from 'react-router-dom';

const RespondentListPage = () => {
    const { user } = useUser();
    const navigate = useNavigate();
    const [assessees, setAssessees] = useState([]);
    const [filters, setFilters] = useState({
        execHead: '',
        dept: '',
        perno: ''
    });
    const [lookups, setLookups] = useState({
        execHeads: [],
        departments: []
    });

    const fetchLookups = useCallback(async () => {
        try {
            const [exRes, deptRes] = await Promise.all([
                axiosAPI.get('/employee/exec-heads'),
                axiosAPI.get('/employee/departments')
            ]);
            setLookups({
                execHeads: exRes.data,
                departments: deptRes.data
            });
        } catch (err) {
            console.error('Failed to fetch lookups', err);
        }
    }, []);

    const fetchAssessees = useCallback(async () => {
        try {
            const response = await axiosAPI.get('/respondent/assessees', {
                params: {
                    adminId: user.userId,
                    isSuperAdmin: user.isSuperAdmin,
                    fy: user.fiscalYear,
                    cycle: user.cycle,
                    ...filters
                }
            });
            setAssessees(response.data);
        } catch (err) {
            console.error('Failed to fetch assessees', err);
        }
    }, [user, filters]);

    useEffect(() => {
        if (user.userId) {
            fetchLookups();
            fetchAssessees();
        }
    }, [user, fetchLookups, fetchAssessees]);

    const handleFilterChange = (field) => (e) => {
        setFilters(prev => ({ ...prev, [field]: e.target.value }));
    };

    const columns = [
        { title: "Perno", field: "Perno", width: 100 },
        { title: "Name", field: "Name", width: 200 },
        { title: "Designation", field: "Designation", width: 200 },
        { title: "Level", field: "Level", width: 80 },
        {
            title: "Submit Status",
            field: "SubmitStatus",
            formatter: (cell) => {
                const val = cell.getValue();
                return val === 'SU' ? '<span style="color: green">Submitted</span>' : '<span style="color: orange">Not Submitted</span>';
            }
        },
        {
            title: "App Status",
            field: "AppStatus",
            formatter: (cell) => {
                const val = cell.getValue();
                if (val === 'AP') return '<span style="color: green">Approved</span>';
                if (val === 'RJ') return '<span style="color: red">Returned</span>';
                return 'Pending';
            }
        },
        {
            title: "Actions",
            formatter: (cell) => {
                const row = cell.getData();
                return `
                    <button class="btn-manage" style="padding: 4px 8px; cursor: pointer; background: #1976d2; color: white; border: none; border-radius: 4px;">
                        Manage Respondents
                    </button>
                `;
            },
            cellClick: (e, cell) => {
                const row = cell.getData();
                navigate(`/manage-respondents/${row.Perno}`);
            },
            hozAlign: "center",
            headerSort: false
        }
    ];

    return (
        <Box sx={{ p: 3 }}>
            <Typography variant="h4" gutterBottom>Add/Remove/Submit Respondent</Typography>

            <Paper sx={{ p: 2, mb: 3 }}>
                <Grid container spacing={2} alignItems="center">
                    <Grid item xs={12} md={3}>
                        <FormControl fullWidth size="small">
                            <InputLabel>Executive Head</InputLabel>
                            <Select
                                value={filters.execHead}
                                label="Executive Head"
                                onChange={handleFilterChange('execHead')}
                            >
                                <MenuItem value="">All</MenuItem>
                                {lookups.execHeads.map(item => (
                                    <MenuItem key={item.Id} value={item.Id}>{item.Text}</MenuItem>
                                ))}
                            </Select>
                        </FormControl>
                    </Grid>
                    <Grid item xs={12} md={3}>
                        <FormControl fullWidth size="small">
                            <InputLabel>Department</InputLabel>
                            <Select
                                value={filters.dept}
                                label="Department"
                                onChange={handleFilterChange('dept')}
                            >
                                <MenuItem value="">All</MenuItem>
                                {lookups.departments.map(item => (
                                    <MenuItem key={item.Id} value={item.Id}>{item.Text}</MenuItem>
                                ))}
                            </Select>
                        </FormControl>
                    </Grid>
                    <Grid item xs={12} md={2}>
                        <TextField
                            fullWidth
                            size="small"
                            label="Perno"
                            value={filters.perno}
                            onChange={handleFilterChange('perno')}
                        />
                    </Grid>
                    <Grid item xs={12} md={2}>
                        <Button variant="contained" onClick={fetchAssessees} fullWidth>Search</Button>
                    </Grid>
                </Grid>
            </Paper>

            <Paper sx={{ p: 2 }}>
                <ReactTabulator
                    data={assessees}
                    columns={columns}
                    layout={"fitColumns"}
                    options={{
                        pagination: "local",
                        paginationSize: 10,
                    }}
                />
            </Paper>
        </Box>
    );
};

export default RespondentListPage;
