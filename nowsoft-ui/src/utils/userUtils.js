import axios from 'axios';

// Function to get browser and device information
export const getUserInfo = async () => {
  const browser = getBrowserInfo();
  const device = navigator.userAgent;
  const ipAddress = await getIpAddress();

  return {
    browser,
    device,
    ipAddress,
  };
};

// Function to determine browser name
const getBrowserInfo = () => {
    const agent = navigator.userAgent.toLowerCase();
  
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

// Function to fetch the IP address using an external service
const getIpAddress = async () => {
  try {
    const response = await axios.get('https://api.ipify.org?format=json');
    return response.data.ip;
  } catch (error) {
    console.error('Error fetching IP address:', error);
    return 'Unknown';
  }
};