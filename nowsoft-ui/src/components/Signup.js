import React, { useState } from 'react';
import axios from 'axios';

const Signup = () => {
  const [formData, setFormData] = useState({
    username: '',
    password: '',
    firstName: '',
    lastName: '',
    device: '',
    browser: '',
    ipAddress: ''
  });

  const [message, setMessage] = useState('');

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post('http://localhost:5180/users/signup', formData);
      setMessage('Signup successful!');
    } catch (error) {
      // Check if error.response is defined and handle different cases
      if (error.response) {
        // Server responded with a status other than 2xx
        console.error('Server responded with an error:', error.response.data);
        setMessage(`Error: ${error.response.data.error || 'Something went wrong.'}`);
      } else if (error.request) {
        // Request was made but no response was received
        console.error('No response received:', error.request);
        setMessage('Error: No response from the server. Please try again later.');
      } else {
        // Something happened in setting up the request
        console.error('Error in request setup:', error.message);
        setMessage(`Error: ${error.message}`);
      }
    }
  };

  return (
    <div>
      <h2>Signup</h2>
      <form onSubmit={handleSubmit}>
        <input type="text" name="username" placeholder="Email" onChange={handleChange} required />
        <input type="password" name="password" placeholder="Password" onChange={handleChange} required />
        <input type="text" name="firstName" placeholder="First Name" onChange={handleChange} required />
        <input type="text" name="lastName" placeholder="Last Name" onChange={handleChange} required />
        <button type="submit">Sign Up</button>
      </form>
      <p>{message}</p>
    </div>
  );
};

export default Signup;
