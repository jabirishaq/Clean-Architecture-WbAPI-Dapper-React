// src/App.js

import React, { useState } from 'react';
import Signup from './components/Signup';
import Login from './components/Login';
import Balance from './components/Balance';

const App = () => {
  const [token, setToken] = useState(null);

  const handleLogin = (jwtToken) => {
    setToken(jwtToken);
  };

  return (
    <div>
      <h1>NowSoft User Management</h1>
      <Signup />
      <Login onLogin={handleLogin} />
      {token && <Balance token={token} />}
    </div>
  );
};

export default App;
