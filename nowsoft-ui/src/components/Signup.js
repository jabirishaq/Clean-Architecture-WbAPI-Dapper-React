import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import { getUserInfo } from '../utils/userUtils';
import '../Form.css';

// Signup component for handling user registration
const Signup = () => {
  // useNavigate hook to programmatically navigate between routes
  const navigate = useNavigate();

  // State to manage form data
  const [formData, setFormData] = useState({
    username: '',
    password: '',
    firstName: '',
    lastName: '',
  });

  // State to manage form validation errors
  const [errors, setErrors] = useState({});

  // Function to validate form fields
  const validate = () => {
    const newErrors = {};
    if (!formData.username) newErrors.username = 'Username is required';
    if (!formData.password) newErrors.password = 'Password is required';
    // Uncomment these lines to enable first and last name validation i required
    // if (!formData.firstName) newErrors.firstName = 'First name is required';
    // if (!formData.lastName) newErrors.lastName = 'Last name is required';
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

      // Send a POST request to the signup endpoint with form data and additional user info
      const response = await axios.post('http://localhost:5180/users/signup', {
        ...formData,
        ...userInfo, // Add device, browser, and IP address info
      });

      // Show success message and navigate to the login page on successful signup
      //toast.success('Signup successful!');

      toast.success('Signup successful!', {
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

      navigate('/login');
    } catch (error) {
      // Handle errors and show appropriate error messages
      if (error.response) {
        toast.error(error.response.data.error || 'Signup failed');
      } else {
        toast.error('Signup failed. Please try again.');
      }
    }
  };

  // Render the signup form
  return (
    <div className="form-container">
      <h2>Signup</h2>
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
        <input
          type="text"
          name="firstName"
          placeholder="First Name"
          value={formData.firstName}
          onChange={handleChange}
        />
        {errors.firstName && <span className="error">{errors.firstName}</span>}
        <input
          type="text"
          name="lastName"
          placeholder="Last Name"
          value={formData.lastName}
          onChange={handleChange}
        />
        {errors.lastName && <span className="error">{errors.lastName}</span>}
        <button type="submit">Sign Up</button>
        <button type="button" onClick={() => navigate('/login')}>
          Already have an account? Login
        </button>
      </form>
    </div>
  );
};

export default Signup;
