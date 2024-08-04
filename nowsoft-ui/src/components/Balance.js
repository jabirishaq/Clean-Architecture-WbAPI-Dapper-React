// Import necessary modules and components
import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import { jwtDecode } from 'jwt-decode'; // Use named import for jwtDecode
import '../Form.css';

// Balance component for displaying user's balance
const Balance = () => {
  // useNavigate hook to programmatically navigate between routes
  const navigate = useNavigate();

  // State to manage user's balance
  const [balance, setBalance] = useState(0);

  // useEffect to fetch user's balance when the component is mounted
  useEffect(() => {
    // Function to decode and check token expiry
    const checkTokenExpiry = () => {
      const token = localStorage.getItem('token');
      if (!token) {
        toast.error('You must be logged in to view your balance.');
        navigate('/login'); // Redirect to login if not logged in
        return false;
      }

      try {
        // Decode the token to get its expiry time
        const decodedToken = jwtDecode(token);
        const currentTime = Date.now() / 1000; // Current time in seconds

        // Check if the token is expired
        if (decodedToken.exp < currentTime) {
          toast.error('Session expired. Please log in again.');
          localStorage.removeItem('token'); // Remove the expired token
          navigate('/login'); // Redirect to login page
          return false;
        }

        return true;
      } catch (error) {
        console.error('Failed to decode token:', error);
        toast.error('Invalid token. Please log in again.');
        localStorage.removeItem('token');
        navigate('/login');
        return false;
      }
    };

    // Async function to fetch user's balance
    const fetchBalance = async () => {
      if (!checkTokenExpiry()) return;

      try {
        const token = localStorage.getItem('token');
        const response = await axios.post(
          'http://localhost:5180/users/auth/balance',
          {},
          {
            headers: {
              Authorization: `Bearer ${token}`, // Include the token in the authorization header
            },
          }
        );

        // Update the state with the retrieved balance
        setBalance(response.data.balance);
      } catch (error) {
        // Log and handle errors by showing appropriate error messages
        console.error('Error fetching balance:', error);
        if (error.response) {
          toast.error('Failed to fetch balance: ' + (error.response.data.message || 'Server error'));
        } else {
          toast.error('Failed to fetch balance. Network error or server did not respond.');
        }
      }
    };

    fetchBalance(); // Call the fetchBalance function to initiate the balance retrieval
  }, [navigate]); // Dependency array with navigate ensures fetchBalance is called once when the component mounts

  // Render the balance information
  return (
    <div className="balance-container">
      <h2>Your Balance is {balance}</h2>
      <button onClick={() => navigate('/login')}>Logout</button> {/* Button to log out and navigate to login page */}
    </div>
  );
};

export default Balance;
