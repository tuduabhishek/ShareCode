import React, { createContext, useState, useContext, useEffect, useCallback } from 'react';
import axiosAPI from '../variables/axiosAPI';

const UserContext = createContext();

export const useUser = () => useContext(UserContext);

export const UserProvider = ({ children }) => {
    const [user, setUser] = useState({
        userId: '',
        userName: '',
        fiscalYear: '',
        cycle: '',
        isSuperAdmin: false,
        role: '',
        isAuthenticated: false,
    });
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState(null);

    const fetchSession = useCallback(async () => {
        try {
            setIsLoading(true);
            const response = await axiosAPI.get('/user/session');
            setUser({ ...response.data, isAuthenticated: true });
            setError(null);
        } catch (err) {
            setError(err.message || 'Failed to fetch session');
            setUser(prev => ({ ...prev, isAuthenticated: false }));
        } finally {
            setIsLoading(false);
        }
    }, []);

    const login = async (userId) => {
        try {
            setIsLoading(true);
            const response = await axiosAPI.get('/user/session', { headers: { 'X-User-ID': userId } });
            setUser({ ...response.data, isAuthenticated: true });
            setError(null);
            return true;
        } catch (err) {
            setError(err.message || 'Login failed');
            return false;
        } finally {
            setIsLoading(false);
        }
    };

    const logout = () => {
        setUser({
            userId: '',
            userName: '',
            fiscalYear: '',
            cycle: '',
            isSuperAdmin: false,
            role: '',
            isAuthenticated: false,
        });
    };

    const value = {
        user,
        setUser,
        isLoading,
        error,
        fetchSession,
        login,
        logout,
    };

    return <UserContext.Provider value={value}>{children}</UserContext.Provider>;
};
