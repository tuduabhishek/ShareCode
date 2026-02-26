import React, { useState, useEffect, useCallback } from 'react';
import { Box, Typography, Paper, Grid, Button, TextField, Radio, RadioGroup, FormControlLabel, FormControl, FormLabel, Card, CardContent, Divider, Alert, List, ListItem, ListItemText, Chip, CircularProgress } from '@mui/material';
import { useUser } from '../context/UserContext';
import axiosAPI from '../variables/axiosAPI';

const FeedbackPage = (props) => {
    const { user } = useUser();
    const [pendingSurveys, setPendingSurveys] = useState([]);
    const [selectedSurvey, setSelectedSurvey] = useState(null);
    const [responses, setResponses] = useState({
        q1a: '1', q1b: '1', q1c: '1', q1d: '1',
        q2a: '', q2b: ''
    });
    const [loading, setLoading] = useState(false);
    const [fetching, setFetching] = useState(false);

    const fetchPending = useCallback(async () => {
        try {
            setFetching(true);
            const urlParams = new URLSearchParams(window.location.search);
            const params = props.mode === 'external'
                ? {
                    respondentId: urlParams.get('id'),
                    assesPno: urlParams.get('assesPno'),
                    fy: urlParams.get('fy'),
                    cycle: urlParams.get('cycle'),
                    otp: urlParams.get('otp')
                }
                : { respondentId: user.userId, fy: user.fiscalYear, cycle: user.cycle };

            if (props.mode === 'external') {
                // If external, we only have one survey to show
                const res = await axiosAPI.get('/survey/details', { params });
                setSelectedSurvey({ ...res.data, ...params });
                setPendingSurveys([]); // Not used in external mode
            } else {
                const res = await axiosAPI.get('/survey/pending', { params });
                setPendingSurveys(res.data);
            }
        } catch (err) {
            console.error(err);
        } finally {
            setFetching(false);
        }
    }, [user, props.mode]);

    useEffect(() => {
        if (props.mode === 'external' || user.userId) fetchPending();
    }, [user, fetchPending, props.mode]);

    const handleSelectSurvey = async (survey) => {
        if (survey.Status === 'Completed') {
            try {
                const res = await axiosAPI.get('/survey/details', {
                    params: { respondentId: user.userId, assesPno: survey.AssesPno, fy: user.fiscalYear, cycle: user.cycle }
                });
                setSelectedSurvey({ ...res.data, Status: 'Completed' });
                setResponses({
                    q1a: res.data.Q1A, q1b: res.data.Q1B, q1c: res.data.Q1C, q1d: res.data.Q1D,
                    q2a: res.data.Q2A, q2b: res.data.Q2B
                });
            } catch (err) {
                alert("Failed to fetch details");
            }
        } else {
            setSelectedSurvey({ ...survey, AssesName: survey.Name });
            setResponses({ q1a: '1', q1b: '1', q1c: '1', q1d: '1', q2a: '', q2b: '' });
        }
    };

    const handleSubmit = async () => {
        if (!responses.q2a || !responses.q2b) {
            alert("Please fill all descriptive questions");
            return;
        }
        try {
            setLoading(true);
            const isExternal = props.mode === 'external';
            const payload = {
                AssesPno: selectedSurvey.AssesPno,
                RespondentPno: isExternal ? selectedSurvey.respondentId : user.userId,
                FiscalYear: isExternal ? selectedSurvey.fy : user.fiscalYear,
                Cycle: isExternal ? selectedSurvey.cycle : user.cycle,
                Q1A: responses.q1a,
                Q1B: responses.q1b,
                Q1C: responses.q1c,
                Q1D: responses.q1d,
                Q2A: responses.q2a,
                Q2B: responses.q2b
            };
            await axiosAPI.post('/survey/submit', payload);
            alert("Feedback submitted successfully");
            setSelectedSurvey(null);
            fetchPending();
        } catch (err) {
            alert("Failed to submit");
        } finally {
            setLoading(false);
        }
    };

    const handleReject = async () => {
        if (!window.confirm("Are you sure you have insufficient exposure to provide feedback?")) return;
        try {
            const isExternal = props.mode === 'external';
            const params = isExternal
                ? { respondentId: selectedSurvey.respondentId, assesPno: selectedSurvey.AssesPno, fy: selectedSurvey.fy, cycle: selectedSurvey.cycle }
                : { respondentId: user.userId, assesPno: selectedSurvey.AssesPno, fy: user.fiscalYear, cycle: user.cycle };

            await axiosAPI.post('/survey/reject', null, { params });
            setSelectedSurvey(null);
            if (!isExternal) fetchPending();
            else alert("Feedback rejected successfully");
        } catch (err) {
            alert("Failed to process request");
        }
    };

    const renderQuestion = (id, label, behaviorLeft, behaviorRight) => (
        <Box sx={{ mb: 4 }}>
            <Typography variant="h6" color="primary">{label}</Typography>
            <Grid container spacing={2} sx={{ mt: 1 }}>
                <Grid item xs={5}>
                    <Typography variant="body2" color="textSecondary">{behaviorLeft}</Typography>
                </Grid>
                <Grid item xs={2} sx={{ display: 'flex', justifyContent: 'center' }}>
                    <RadioGroup
                        row
                        value={responses[id]}
                        onChange={(e) => setResponses({ ...responses, [id]: e.target.value })}
                        disabled={selectedSurvey?.Status === 'Completed'}
                    >
                        <Radio value="1" size="small" />
                        <Radio value="2" size="medium" />
                        <Radio value="3" size="large" />
                    </RadioGroup>
                </Grid>
                <Grid item xs={5}>
                    <Typography variant="body2" color="textSecondary">{behaviorRight}</Typography>
                </Grid>
            </Grid>
            <Divider sx={{ mt: 2 }} />
        </Box>
    );

    return (
        <Box sx={{ p: 3 }}>
            <Typography variant="h4" gutterBottom>360 Degree Feedback Survey</Typography>

            <Grid container spacing={3}>
                <Grid item xs={12} md={4}>
                    <Paper sx={{ p: 2 }}>
                        <Typography variant="h6" gutterBottom>Surveys for you</Typography>
                        {fetching ? <CircularProgress size={24} /> : (
                            <List>
                                {pendingSurveys.map(s => (
                                    <ListItem
                                        key={s.AssesPno}
                                        button
                                        onClick={() => handleSelectSurvey(s)}
                                        selected={selectedSurvey?.AssesPno === s.AssesPno}
                                    >
                                        <ListItemText primary={s.Name} secondary={s.Designation} />
                                        <Chip
                                            label={s.Status}
                                            color={s.Status === 'Completed' ? 'success' : 'warning'}
                                            size="small"
                                        />
                                    </ListItem>
                                ))}
                            </List>
                        )}
                    </Paper>
                </Grid>

                <Grid item xs={12} md={8}>
                    {selectedSurvey ? (
                        <Paper sx={{ p: 3 }}>
                            <Typography variant="h5" color="primary">Rate: {selectedSurvey.AssesName}</Typography>
                            <Typography variant="body2" sx={{ mb: 3, fontStyle: 'italic' }}>
                                Scale: Left (Negative), Center (Neutral/Good), Right (Exemplary agile behavior).
                            </Typography>

                            {renderQuestion('q1a', 'Ownership',
                                'Passes on the onus of decision making to others. Does not deliver on committed outcomes.',
                                'Proactively takes decisions. Finds it, owns it, fixes it.')}

                            {renderQuestion('q1b', 'Collaboration',
                                'Holds back in sharing resources. Does not acknowledge others\' contributions.',
                                'Proactively volunteers to share resources. Promotes recognition of others.')}

                            {renderQuestion('q1c', 'Responsiveness',
                                'Holds multiple meetings without initiating action. Not open to change.',
                                'Moves quickly to action. Is a change champion.')}

                            {renderQuestion('q1d', 'People Development',
                                'Promotes a transactional culture. Holds talent in the same role.',
                                'Promotes open trust-based culture. Coaches and enables team members.')}

                            <Box sx={{ mt: 3 }}>
                                <Typography variant="h6">Descriptive Feedback</Typography>
                                <TextField
                                    fullWidth
                                    multiline
                                    rows={4}
                                    label={`What have you seen ${selectedSurvey.AssesName} do that is exemplary?`}
                                    value={responses.q2a}
                                    onChange={(e) => setResponses({ ...responses, q2a: e.target.value.substring(0, 500) })}
                                    disabled={selectedSurvey?.Status === 'Completed'}
                                    sx={{ mt: 2 }}
                                    helperText={`${responses.q2a.length}/500`}
                                />
                                <TextField
                                    fullWidth
                                    multiline
                                    rows={4}
                                    label={`How can ${selectedSurvey.AssesName} improve on behaviors?`}
                                    value={responses.q2b}
                                    onChange={(e) => setResponses({ ...responses, q2b: e.target.value.substring(0, 500) })}
                                    disabled={selectedSurvey?.Status === 'Completed'}
                                    sx={{ mt: 2 }}
                                    helperText={`${responses.q2b.length}/500`}
                                />
                            </Box>

                            {selectedSurvey.Status !== 'Completed' && (
                                <Box sx={{ mt: 4, display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
                                    <Button variant="outlined" color="error" onClick={handleReject}>Insufficient Exposure</Button>
                                    <Button variant="contained" color="primary" onClick={handleSubmit} disabled={loading}>Submit Feedback</Button>
                                </Box>
                            )}
                        </Paper>
                    ) : (
                        <Alert severity="info" variant="outlined">Select an assessee from the left to provide feedback.</Alert>
                    )}
                </Grid>
            </Grid>
        </Box>
    );
};

export default FeedbackPage;
