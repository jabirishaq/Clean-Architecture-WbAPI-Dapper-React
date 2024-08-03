import React, { useState, useEffect } from 'react';
import axios from 'axios';

const Balance = ({ token }) => {
  const [balance, setBalance] = useState(0);

  useEffect(() => {
    console.log('Token:', token);

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
        console.log('API Response:', response.data.balance);
        setBalance(response.data.balance);
      } catch (error) {
        console.error('Error fetching balance:', error);
        if (error.response) {
          console.error('Response Error:', error.response.data);
        }
      }
    };

    if (token) {
      fetchBalance();
    }
  }, [token]);

  useEffect(() => {
    console.log('Updated Balance:', balance);
  }, [balance]);

  return (
    <div>
      <h2>Your Balance is</h2>
      {balance !== null ? <p>{balance}</p> : <p>Loading...</p>}
    </div>
  );
};

export default Balance;
