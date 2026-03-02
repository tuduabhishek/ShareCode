import React, { useState, useEffect, useRef } from 'react';
import { Box, Typography, Paper, Grid, TextField, Button, Alert, CircularProgress, Divider } from '@mui/material';
import { TabulatorFull as Tabulator } from 'tabulator-tables';
import 'tabulator-tables/dist/css/tabulator_semanticui.min.css';
import { useUser } from '../context/UserContext';
import axiosAPI from '../variables/axiosAPI';

const BulkRecordUpdatePage = () => {
    const { user } = useUser();
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [filters, setFilters] = useState({ year: user.fiscalYear, perno: '' });

    const editorRef = useRef(null);

    const loadEditorData = async () => {
        if (!filters.perno) return alert("Please enter an Assessee PNo");
        setLoading(true);
        setError(null);
        try {
            const res = await axiosAPI.get(`/reports/raw-data?perno=${filters.perno}&year=${filters.year}`);
            new Tabulator(editorRef.current, {
                data: res.data,
                layout: "fitColumns",
                columns: [
                    { title: "ID", field: "SS_ID", width: 80 },
                    { title: "Giver PNo", field: "SS_PNO", width: 100 },
                    { title: "Email", field: "SS_EMAIL", editor: "input" },
                    { title: "Status", field: "SS_STATUS", editor: "input", width: 80 },
                    { title: "Tag", field: "SS_TAG", editor: "input", width: 60 },
                    { title: "App Tag", field: "SS_APP_TAG", editor: "input", width: 80 },
                    { title: "Wfl", field: "SS_WFL_STATUS", editor: "input", width: 60 },
                    { title: "Action", formatter: "buttonTick", cellClick: (e, cell) => handleUpdateRecord(cell.getData()), headerSort: false, width: 80 }
                ]
            });
        } catch (err) { setError("Failed to load editor data"); }
        finally { setLoading(false); }
    };

    const handleUpdateRecord = async (record) => {
        try {
            await axiosAPI.post('/admin/update-record', {
                ...record,
                FiscalYear: filters.year,
                AssesPno: filters.perno,
                Perno: record.SS_PNO,
                Id: record.SS_ID,
                Email: record.SS_EMAIL,
                Status: record.SS_STATUS,
                Tag: record.SS_TAG,
                AppTag: record.SS_APP_TAG,
                Level: record.SS_LEVEL,
                WflStatus: record.SS_WFL_STATUS
            });
            alert("Record updated successfully");
        } catch (err) { alert("Update failed"); }
    };

    return (
        <Box sx={{ py: 4 }}>
            <Typography variant="h4" gutterBottom color="primary">Bulk Record Update</Typography>
            <Typography variant="subtitle1" gutterBottom color="textSecondary">
                Edit raw survey status records for a specific assessee in the selected fiscal year.
            </Typography>

            <Paper sx={{ p: 3, mb: 4 }}>
                <Grid container spacing={2} alignItems="center">
                    <Grid item xs={3}><TextField label="Year" fullWidth size="small" value={filters.year} onChange={(e) => setFilters({ ...filters, year: e.target.value })} /></Grid>
                    <Grid item xs={4}><TextField label="Assessee PNo" fullWidth size="small" value={filters.perno} onChange={(e) => setFilters({ ...filters, perno: e.target.value })} /></Grid>
                    <Grid item xs={3}><Button variant="contained" fullWidth onClick={loadEditorData}>Display Data</Button></Grid>
                </Grid>
            </Paper>

            {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
            {loading && <Box sx={{ display: 'flex', justifyContent: 'center', p: 3 }}><CircularProgress /></Box>}

            <Paper sx={{ p: 2 }}>
                <div ref={editorRef} style={{ height: '500px' }}></div>
            </Paper>
        </Box>
    );
};

export default BulkRecordUpdatePage;
