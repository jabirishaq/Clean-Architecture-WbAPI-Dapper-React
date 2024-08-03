// src/components/Balance.js

import React, { useState, useEffect } from 'react';
import axios from 'axios';

const Balance = ({ token }) => {
  const [balance, setBalance] = useState(null);

  useEffect(() => {
    const fetchBalance = async () => {
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
        setBalance(response.data.Balance);
      } catch (error) {
        console.error('Error fetching balance:', error);
      }
    };

    if (token) {
      fetchBalance();
    }
  }, [token]);

  return (
    <div>
      <h2>Your Balance</h2>
      {balance !== null ? <p>{balance}</p> : <p>Loading...</p>}
    </div>
  );
};

export default Balance;
