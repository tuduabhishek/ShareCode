import React, { useState, useEffect } from 'react';
import { Box, Typography, Paper, Tabs, Tab, Grid, TextField, Button, MenuItem, CircularProgress, Alert } from '@mui/material';
import { TabulatorFull as Tabulator } from 'tabulator-tables';
import 'tabulator-tables/dist/css/tabulator_semanticui.min.css';
import { useUser } from '../context/UserContext';
import axiosAPI from '../variables/axiosAPI';

const ReportingPage = () => {
    const { user } = useUser();
    const [tab, setTab] = useState(0);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [filters, setFilters] = useState({ year: user.fiscalYear, cycle: user.cycle, pno: '' });
    const [scores, setScores] = useState([]);
    const [comments, setComments] = useState({ SelfStrengths: [], OtherStrengths: [], SelfDevelop: [], OtherDevelop: [] });

    const tableRef = React.useRef(null);
    const tabulatorInstance = React.useRef(null);

    const loadIndividualReport = async () => {
        if (!filters.pno) {
            setError("Please enter an Assessee PNo to view individual report.");
            return;
        }
        setLoading(true);
        setError(null);
        try {
            const [sRes, cRes] = await Promise.all([
                axiosAPI.get('/reports/individual-scores', { params: { perno: filters.pno, year: filters.year } }),
                axiosAPI.get('/reports/individual-comments', { params: { perno: filters.pno, year: filters.year } })
            ]);
            setScores(sRes.data);
            setComments(cRes.data);
        } catch (err) {
            setError("Failed to fetch individual report data");
        } finally {
            setLoading(false);
        }
    };

    const loadReport = async () => {
        setLoading(true);
        setError(null);
        try {
            const endpoint = tab === 0 ? '/reports/detailed-status' : '/reports/summary-completion';
            const res = await axiosAPI.get(endpoint, { params: filters });

            const columns = tab === 0 ? [
                { title: "Assessee PNo", field: "assesor_pno", headerFilter: "input" },
                { title: "Assessee Name", field: "assesor_name" },
                { title: "Giver PNo", field: "feedback_giver_pno" },
                { title: "Giver Name", field: "feedback_giver_name" },
                { title: "Category", field: "category" },
                { title: "Status", field: "status" },
                { title: "Approval", field: "approval_status" }
            ] : [
                { title: "Assessee PNo", field: "ss_asses_pno", headerFilter: "input" },
                { title: "Category", field: "CATEGORY" },
                { title: "Min Reqd", field: "MIN_REQD" },
                { title: "Approved", field: "APPROVED_COUNT" },
                { title: "Completed", field: "COMPLETED_COUNT" },
                { title: "Rejected", field: "INSUFFICIENT_EXPOSURE" },
                {
                    title: "Criteria", field: "CRITERIA", formatter: (cell) => {
                        const val = cell.getValue();
                        cell.getElement().style.color = val === 'OK' ? 'green' : 'red';
                        return val;
                    }
                }
            ];

            if (tabulatorInstance.current) tabulatorInstance.current.destroy();
            tabulatorInstance.current = new Tabulator(tableRef.current, {
                data: res.data,
                layout: "fitColumns",
                pagination: "local",
                paginationSize: 20,
                columns: columns,
                downloadDataFormatter: (data) => data,
                downloadReady: (fileContents, blob) => blob
            });
        } catch (err) {
            setError("Failed to fetch report data");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        if (tab < 2) loadReport();
        else if (tab === 2) loadIndividualReport();
    }, [tab]);

    const getGrade = (val) => {
        if (!val) return "N/A";
        if (val <= 1.6) return "Unacceptable";
        if (val <= 2.6) return "Acceptable";
        return "Gold Standard";
    };

    const handleDownload = () => {
        if (tabulatorInstance.current) tabulatorInstance.current.download("csv", `report_${tab === 0 ? 'detailed' : 'summary'}.csv`);
    };

    return (
        <Box sx={{ py: 4 }}>
            <Typography variant="h4" gutterBottom color="primary">Reporting Dashboard</Typography>

            <Paper sx={{ mb: 3 }}>
                <Tabs value={tab} onChange={(e, n) => setTab(n)}>
                    <Tab label="Detailed Status Report" />
                    <Tab label="Completion Summary" />
                    <Tab label="Individual Report" />
                </Tabs>
            </Paper>

            <Paper sx={{ p: 2, mb: 3 }}>
                <Grid container spacing={2} alignItems="center">
                    <Grid item xs={2}><TextField label="Year" fullWidth value={filters.year} onChange={(e) => setFilters({ ...filters, year: e.target.value })} /></Grid>
                    <Grid item xs={2}><TextField label="Cycle" fullWidth value={filters.cycle} onChange={(e) => setFilters({ ...filters, cycle: e.target.value })} /></Grid>
                    <Grid item xs={3}><TextField label="Assessee PNo (Required for Indiv.)" fullWidth value={filters.pno} onChange={(e) => setFilters({ ...filters, pno: e.target.value })} /></Grid>
                    <Grid item xs={2}><Button variant="contained" fullWidth onClick={tab < 2 ? loadReport : loadIndividualReport}>Refresh</Button></Grid>
                    <Grid item xs={3}><Button variant="outlined" fullWidth onClick={handleDownload} disabled={tab === 2 || !tableRef.current}>Export CSV</Button></Grid>
                </Grid>
            </Paper>

            {tab === 2 && scores.length > 0 && (
                <Box sx={{ mt: 3, mb: 5 }}>
                    <Typography variant="h6" gutterBottom>Behavioral Aspect Scores</Typography>
                    <Paper sx={{ mb: 4, overflow: 'hidden' }}>
                        <Grid container sx={{ bgcolor: 'primary.main', color: 'white', p: 1, fontWeight: 'bold' }}>
                            <Grid item xs={3}>Category</Grid>
                            <Grid item xs={2}>Accountability</Grid>
                            <Grid item xs={2}>Collaboration</Grid>
                            <Grid item xs={2}>Responsiveness</Grid>
                            <Grid item xs={3}>People Development</Grid>
                        </Grid>
                        {scores.map(s => (
                            <Grid container key={s.Category} sx={{ p: 1, borderTop: '1px solid #eee', fontSize: '0.9rem' }}>
                                <Grid item xs={3}><strong>{s.Category}</strong> ({s.Count})</Grid>
                                <Grid item xs={2}>{getGrade(s.Accountability)} ({s.Accountability})</Grid>
                                <Grid item xs={2}>{getGrade(s.Collaboration)} ({s.Collaboration})</Grid>
                                <Grid item xs={2}>{getGrade(s.Responsiveness)} ({s.Responsiveness})</Grid>
                                <Grid item xs={3}>{getGrade(s.PeopleDevelopment)} ({s.PeopleDevelopment})</Grid>
                            </Grid>
                        ))}
                    </Paper>

                    <Typography variant="h6" gutterBottom>Qualitative Feedback</Typography>
                    <Grid container spacing={3}>
                        <Grid item xs={6}>
                            <Paper sx={{ p: 2, bgcolor: '#f0f7ff' }}>
                                <Typography variant="subtitle1" color="primary"><strong>Strengths / Continuous Behaviors</strong></Typography>
                                <Typography variant="body2" sx={{ mt: 1, color: 'text.secondary' }}>Self View:</Typography>
                                <ul>{comments.SelfStrengths.map((c, i) => <li key={i}>{c}</li>)}</ul>
                                <Typography variant="body2" sx={{ mt: 1, color: 'text.secondary' }}>Others:</Typography>
                                <ul>{comments.OtherStrengths.map((c, i) => <li key={i}>{c}</li>)}</ul>
                            </Paper>
                        </Grid>
                        <Grid item xs={6}>
                            <Paper sx={{ p: 2, bgcolor: '#fff0f0' }}>
                                <Typography variant="subtitle1" color="secondary"><strong>Areas for Improvement</strong></Typography>
                                <Typography variant="body2" sx={{ mt: 1, color: 'text.secondary' }}>Self View:</Typography>
                                <ul>{comments.SelfDevelop.map((c, i) => <li key={i}>{c}</li>)}</ul>
                                <Typography variant="body2" sx={{ mt: 1, color: 'text.secondary' }}>Others:</Typography>
                                <ul>{comments.OtherDevelop.map((c, i) => <li key={i}>{c}</li>)}</ul>
                            </Paper>
                        </Grid>
                    </Grid>
                </Box>
            )}

            <div ref={tableRef} style={{ display: tab === 2 ? 'none' : 'block' }}></div>
        </Box>
    );
};

export default ReportingPage;
