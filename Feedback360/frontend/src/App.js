import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { ThemeProvider, createTheme, CssBaseline } from '@mui/material';
import { UserProvider, useUser } from './context/UserContext';
import MainLayout from './components/MainLayout';
import LoginPage from './pages/LoginPage';
import UserCreationPage from './pages/UserCreationPage';
import AddEmployeePage from './pages/AddEmployeePage';
import RespondentListPage from './pages/RespondentListPage';
import ManageRespondentsPage from './pages/ManageRespondentsPage';
import ApprovalPage from './pages/ApprovalPage';
import FeedbackPage from './pages/FeedbackPage';
import SelectAssessorPage from './pages/SelectAssessorPage';
import SurveyEntryPage from './pages/SurveyEntryPage';
import ReportingPage from './pages/ReportingPage';
import ChangeBUHRPage from './pages/ChangeBUHRPage';
import ChangeApproverPage from './pages/ChangeApproverPage';
import MasterDataPage from './pages/MasterDataPage';
import BulkRecordUpdatePage from './pages/BulkRecordUpdatePage';
import SendMailPage from './pages/SendMailPage';
import SuperAdminPage from './pages/SuperAdminPage';

const theme = createTheme({
    palette: {
        primary: { main: '#1976d2' },
        secondary: { main: '#dc004e' },
    },
});

const PrivateRoute = ({ children }) => {
    const { user, isLoading } = useUser();
    if (isLoading) return null;
    return user.isAuthenticated ? children : <Navigate to="/login" />;
};

const AppRoutes = () => {
    return (
        <Routes>
            <Route path="/login" element={<LoginPage />} />
            <Route path="/" element={<PrivateRoute><MainLayout /></PrivateRoute>}>
                <Route index element={<Navigate to="/feedback" />} />
                <Route path="feedback" element={<FeedbackPage />} />
                <Route path="add-employee" element={<AddEmployeePage />} />
                <Route path="user-creation" element={<UserCreationPage />} />
                <Route path="respondents" element={<RespondentListPage />} />
                <Route path="manage-respondents/:perno" element={<ManageRespondentsPage />} />
                <Route path="select-assessors" element={<SelectAssessorPage />} />
                <Route path="approval" element={<ApprovalPage />} />
                <Route path="reporting" element={<ReportingPage />} />

                {/* Administrative Pages */}
                <Route path="change-buhr" element={<ChangeBUHRPage />} />
                <Route path="change-approver" element={<ChangeApproverPage />} />
                <Route path="master-data" element={<MasterDataPage />} />
                <Route path="bulk-update" element={<BulkRecordUpdatePage />} />
                <Route path="send-mail" element={<SendMailPage />} />
                <Route path="super-admin" element={<SuperAdminPage />} />
            </Route>
            <Route path="/survey-entry" element={<SurveyEntryPage />} />
            <Route path="/feedback-form" element={<FeedbackPage mode="external" />} />
        </Routes>
    );
};

function App() {
    return (
        <ThemeProvider theme={theme}>
            <CssBaseline />
            <UserProvider>
                <Router>
                    <AppRoutes />
                </Router>
            </UserProvider>
        </ThemeProvider>
    );
}

export default App;
