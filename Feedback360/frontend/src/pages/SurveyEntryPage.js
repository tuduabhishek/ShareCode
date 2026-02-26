import React, { useState } from 'react';
import { Box, Typography, Paper, TextField, Button, Alert, Container, Card, CardContent } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import axiosAPI from '../variables/axiosAPI';

const SurveyEntryPage = () => {
    const [id, setId] = useState('');
    const [otp, setOtp] = useState('');
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();

    const handleVerify = async () => {
        try {
            setLoading(true);
            setError('');
            const res = await axiosAPI.post('/survey/verify-otp', null, { params: { id, otp } });
            // Store minimal session for this survey
            const { assesPno, fy, cycle } = res.data;
            // Store minimal session for this survey
            sessionStorage.setItem('surveySession', JSON.stringify(res.data));
            // Navigate to actual survey form with all necessary context
            navigate(`/feedback-form?id=${id}&otp=${otp}&assesPno=${assesPno}&fy=${fy}&cycle=${cycle}`);
        } catch (err) {
            setError('Invalid ID or OTP. Please check your email.');
        } finally {
            setLoading(false);
        }
    };

    return (
        <Container maxWidth="sm">
            <Box sx={{ mt: 8 }}>
                <Card elevation={3}>
                    <CardContent sx={{ p: 4 }}>
                        <Typography variant="h4" gutterBottom align="center" color="primary">360 Feedback Survey</Typography>
                        <Typography variant="body1" align="center" sx={{ mb: 4 }}>
                            Please enter your ID and the One-Time Password (OTP) sent to your email to start the survey.
                        </Typography>

                        {error && <Alert severity="error" sx={{ mb: 3 }}>{error}</Alert>}

                        <TextField
                            fullWidth
                            label="Respondent ID"
                            value={id}
                            onChange={(e) => setId(e.target.value)}
                            margin="normal"
                        />
                        <TextField
                            fullWidth
                            label="OTP / Ref No"
                            type="password"
                            value={otp}
                            onChange={(e) => setOtp(e.target.value)}
                            margin="normal"
                        />

                        <Button
                            fullWidth
                            variant="contained"
                            size="large"
                            onClick={handleVerify}
                            disabled={loading}
                            sx={{ mt: 4, py: 1.5 }}
                        >
                            {loading ? 'Verifying...' : 'Access Survey'}
                        </Button>
                    </CardContent>
                </Card>
            </Box>
        </Container>
    );
};

export default SurveyEntryPage;
