// src/components/Balance.js
import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import '../Form.css';

const Balance = () => {
  const navigate = useNavigate();
  const [balance, setBalance] = useState(0);

  useEffect(() => {
    const fetchBalance = async () => {
      const token = localStorage.getItem('token');
      if (!token) {
        toast.error('You must be logged in to view your balance.');
        navigate('/login'); // Redirect to login if not logged in
        return;
      }

      try {
        const response = await axios.post(
          'http://localhost:5180/users/auth/balance',
          {},
          {
            headers: {
              Authorization: `Bearer ${token}`
            }
          }
        );
        setBalance(response.data.balance);
      } catch (error) {
        console.error('Error fetching balance:', error);
        if (error.response) {
          toast.error('Failed to fetch balance: ' + (error.response.data.message || 'Server error'));
        } else {
          toast.error('Failed to fetch balance. Network error or server did not respond.');
        }
      }
    };

    fetchBalance();
  }, [navigate]);

  return (
    <div className="balance-container">
      <h2>Your Balance is {balance}</h2>
      <button onClick={() => navigate('/login')}>Logout</button>
    </div>
  );
};

export default Balance;
