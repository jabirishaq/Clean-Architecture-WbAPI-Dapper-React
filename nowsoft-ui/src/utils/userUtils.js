// Import axios for making HTTP requests
import axios from 'axios';

// Function to get browser and device information
// This function returns an object containing the browser name, device information, and IP address
export const getUserInfo = async () => {
  const browser = getBrowserInfo(); // Determine the browser name
  const device = getDeviceInfo();   // Determine the device information
  const ipAddress = await getIpAddress(); // Fetch the user's IP address

  return {
    browser,
    device,
    ipAddress,
  };
};

// Function to determine browser name based on the user agent string
const getBrowserInfo = () => {
  const agent = navigator.userAgent.toLowerCase();

  // Check user agent string for known browser identifiers
  if (agent.indexOf('firefox') > -1) {
    return 'Firefox';
  } else if (agent.indexOf('edg') > -1) { // Microsoft Edge
    return 'Edge';
  } else if (agent.indexOf('opr') > -1 || agent.indexOf('opera') > -1) { // Opera
    return 'Opera';
  } else if (agent.indexOf('chrome') > -1 && agent.indexOf('safari') > -1 && agent.indexOf('opr') === -1 && agent.indexOf('edg') === -1) {
    return 'Chrome';
  } else if (agent.indexOf('safari') > -1 && agent.indexOf('chrome') === -1 && agent.indexOf('opr') === -1 && agent.indexOf('edg') === -1) {
    return 'Safari';
  } else if (agent.indexOf('msie') > -1 || agent.indexOf('trident') > -1) {
    return 'Internet Explorer';
  } else {
    return 'Unknown';
  }
};

// Function to extract device information, specifically for Windows PCs
// This function attempts to identify the Windows version from the user agent string
const getDeviceInfo = () => {
  const agent = navigator.userAgent;
  const windowsMatch = agent.match(/windows nt ([\d.]+)/i); // Regular expression to find Windows version

  if (windowsMatch) {
    const version = windowsMatch[1]; // Extract the Windows version number
    return `PC Windows ${getWindowsVersion(version)}`; // Convert version number to human-readable form
  }
  return 'Unknown Device';
};

// Helper function to convert Windows version numbers to more readable names
// Currently only converts Windows 10 to Windows 11 for simplification
const getWindowsVersion = (version) => {
  switch (version) {
    case '10.0':
      return '11';
    default:
      return version; // Return the version number if not specifically mapped
  }
};

// Function to fetch the IP address using an external service (ipify API)
// This function makes an HTTP GET request to retrieve the user's public IP address
const getIpAddress = async () => {
  try {
    const response = await axios.get('https://api.ipify.org?format=json'); // Call ipify API for IP address
    return response.data.ip; // Return the IP address from the response
  } catch (error) {
    console.error('Error fetching IP address:', error); // Log error if fetching fails
    return 'Unknown'; // Return 'Unknown' if there is an error
  }
};
