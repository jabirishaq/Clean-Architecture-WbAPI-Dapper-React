import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import { getUserInfo } from '../utils/userUtils';
import '../Form.css';

// Login component for handling user login
const Login = () => {
  // useNavigate hook to programmatically navigate between routes
  const navigate = useNavigate();

  // State to manage form data
  const [formData, setFormData] = useState({
    username: '',
    password: '',
  });

  // State to manage form validation errors
  const [errors, setErrors] = useState({});

  // Function to validate form fields
  const validate = () => {
    const newErrors = {};
    if (!formData.username) newErrors.username = 'Username is required';
    if (!formData.password) newErrors.password = 'Password is required';
    return newErrors;
  };

  // Handle input change events to update form state
  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value, // Update the state with new input value
    });
    setErrors({ ...errors, [e.target.name]: '' }); // Clear error message for the input
  };

  // Handle form submission
  const handleSubmit = async (e) => {
    e.preventDefault(); // Prevent default form submission
    const newErrors = validate(); // Validate form inputs

    // If there are validation errors, update error state and exit
    if (Object.keys(newErrors).length > 0) {
      setErrors(newErrors);
      return;
    }

    try {
      // Get additional user information (e.g., device, browser, IP address)
      const userInfo = await getUserInfo();

      // Send a POST request to the authenticate endpoint with form data and additional user info
      const response = await axios.post('http://localhost:5180/users/authenticate', {
        ...formData,
        ...userInfo, // Add device, browser, and IP address info
      });

      // Save the JWT token in local storage
      localStorage.setItem('token', response.data.token); // Save token

      // Show success message and navigate to the balance page on successful login
      //toast.success('Login successful!');
      
      toast.success('Login successful!', {
        className: 'toast-custom',
        bodyClassName: 'toast-custom',
        position: "top-right",
        autoClose: 5000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        progress: undefined,
      });

      navigate('/balance'); // Navigate to balance page
    } catch (error) {
      // Handle errors and show appropriate error messages
      if (error.response) {
        toast.error(error.response.data.message || 'Login failed');
      } else {
        toast.error('Login failed. Please try again.');
      }
    }
  };

  // Render the login form
  return (
    <div className="form-container">
      <h2>Login</h2>
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          name="username"
          placeholder="Username"
          value={formData.username}
          onChange={handleChange}
        />
        {errors.username && <span className="error">{errors.username}</span>}
        <input
          type="password"
          name="password"
          placeholder="Password"
          value={formData.password}
          onChange={handleChange}
        />
        {errors.password && <span className="error">{errors.password}</span>}
        <button type="submit">Log In</button>
        <button type="button" onClick={() => navigate('/signup')}>
          Don't have an account? Signup
        </button>
      </form>
    </div>
  );
};

export default Login;
