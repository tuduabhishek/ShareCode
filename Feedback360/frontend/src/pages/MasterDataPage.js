import React, { useState, useEffect, useCallback, useRef } from 'react';
import { Box, Typography, Paper, Grid, TextField, Button, MenuItem, Alert, CircularProgress, Divider } from '@mui/material';
import { TabulatorFull as Tabulator } from 'tabulator-tables';
import 'tabulator-tables/dist/css/tabulator_semanticui.min.css';
import { useUser } from '../context/UserContext';
import axiosAPI from '../variables/axiosAPI';

const MasterDataPage = () => {
    const { user } = useUser();
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(null);
    const [filters, setFilters] = useState({ year: user.fiscalYear, cycle: user.cycle, execHead: '0', sgrade: '0', buhr: '', perno: '' });
    const [lookups, setLookups] = useState({ execHeads: [] });
    const [uploadFile, setUploadFile] = useState(null);

    const potentialRef = useRef(null);
    const excludedRef = useRef(null);

    const fetchLookups = useCallback(async () => {
        try {
            const res = await axiosAPI.get(`/employee/lookups?adminId=${user.userId}&isSuperAdmin=true`);
            setLookups({ execHeads: res.data.ExecHeads });
        } catch (err) { console.error(err); }
    }, [user.userId]);

    useEffect(() => { fetchLookups(); }, [fetchLookups]);

    const loadData = async () => {
        setLoading(true);
        setError(null);
        try {
            const [pRes, eRes] = await Promise.all([
                axiosAPI.get('/admin/potential-exclusions', { params: filters }),
                axiosAPI.get('/admin/excluded', { params: filters })
            ]);

            new Tabulator(potentialRef.current, {
                data: pRes.data,
                layout: "fitColumns",
                placeholder: "No potential exclusions found",
                columns: [
                    { title: "PNo", field: "Perno", width: 100 },
                    { title: "Name", field: "EmployeeName" },
                    { title: "Dept", field: "DeptDesc" },
                    { title: "Action", formatter: "buttonTick", cellClick: (e, cell) => handleExclude(cell.getData()), headerSort: false, width: 80 }
                ]
            });

            new Tabulator(excludedRef.current, {
                data: eRes.data,
                layout: "fitColumns",
                placeholder: "No excluded employees found",
                columns: [
                    { title: "PNo", field: "Perno", width: 100 },
                    { title: "Name", field: "EmployeeName" },
                    { title: "Action", formatter: "buttonCross", cellClick: (e, cell) => handleInclude(cell.getData()), headerSort: false, width: 80 }
                ]
            });
        } catch (err) { setError("Failed to load data"); }
        finally { setLoading(false); }
    };

    const handleExclude = async (emp) => {
        try {
            await axiosAPI.post('/admin/add-exclusions', [emp], { params: { me: user.userId } });
            loadData();
        } catch (err) { alert("Exclusion failed"); }
    };

    const handleInclude = async (emp) => {
        try {
            await axiosAPI.post('/admin/remove-exclusions', [emp]);
            loadData();
        } catch (err) { alert("Removal failed"); }
    };

    const handleBulkUpload = async () => {
        if (!uploadFile) return alert("Please select a JSON manifest");
        const reader = new FileReader();
        reader.onload = async (e) => {
            try {
                const data = JSON.parse(e.target.result);
                await axiosAPI.post('/admin/bulk-upload-master', data);
                setSuccess("Manifest upload successful");
                setUploadFile(null);
            } catch (err) { setError("Failed to parse/upload manifest"); }
        };
        reader.readAsText(uploadFile);
    };

    return (
        <Box sx={{ py: 4 }}>
            <Typography variant="h4" gutterBottom color="primary">Master Data Management</Typography>

            <Paper sx={{ p: 3, mb: 4 }}>
                <Typography variant="h6" gutterBottom>Bulk Upload Employee Master</Typography>
                <Divider sx={{ mb: 2 }} />
                <Grid container spacing={2} alignItems="center">
                    <Grid item xs={8}>
                        <input
                            type="file"
                            accept=".json"
                            onChange={(e) => setUploadFile(e.target.files[0])}
                            style={{ width: '100%' }}
                        />
                    </Grid>
                    <Grid item xs={4}>
                        <Button variant="contained" fullWidth onClick={handleBulkUpload} disabled={!uploadFile}>Upload Manifest</Button>
                    </Grid>
                </Grid>
            </Paper>

            <Paper sx={{ p: 3, mb: 4 }}>
                <Typography variant="h6" gutterBottom>Exclusion Management Filters</Typography>
                <Divider sx={{ mb: 2 }} />
                <Grid container spacing={2} alignItems="center">
                    <Grid item xs={2}><TextField label="Year" fullWidth size="small" value={filters.year} onChange={(e) => setFilters({ ...filters, year: e.target.value })} /></Grid>
                    <Grid item xs={2}><TextField label="Cycle" fullWidth size="small" value={filters.cycle} onChange={(e) => setFilters({ ...filters, cycle: e.target.value })} /></Grid>
                    <Grid item xs={3}>
                        <TextField select label="Exec Head" fullWidth size="small" value={filters.execHead} onChange={(e) => setFilters({ ...filters, execHead: e.target.value })}>
                            <MenuItem value="0">All</MenuItem>
                            {lookups.execHeads.map(o => <MenuItem key={o.Id} value={o.Id}>{o.Text}</MenuItem>)}
                        </TextField>
                    </Grid>
                    <Grid item xs={2}><TextField label="PNo" fullWidth size="small" value={filters.perno} onChange={(e) => setFilters({ ...filters, perno: e.target.value })} /></Grid>
                    <Grid item xs={3}><Button variant="contained" fullWidth onClick={loadData}>Display Data</Button></Grid>
                </Grid>
            </Paper>

            {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
            {success && <Alert severity="success" sx={{ mb: 2 }}>{success}</Alert>}

            <Grid container spacing={3}>
                <Grid item xs={12} md={6}>
                    <Typography variant="h6" gutterBottom>Potential Exclusions</Typography>
                    <div ref={potentialRef} style={{ height: '400px' }}></div>
                </Grid>
                <Grid item xs={12} md={6}>
                    <Typography variant="h6" gutterBottom>Excluded Employees</Typography>
                    <div ref={excludedRef} style={{ height: '400px' }}></div>
                </Grid>
            </Grid>
        </Box>
    );
};

export default MasterDataPage;
