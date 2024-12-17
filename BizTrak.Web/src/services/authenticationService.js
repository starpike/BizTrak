import { jwtDecode } from 'jwt-decode'

const authenticationService = {
    isTokenExpired: (token) => {
        try {
          const decoded = jwtDecode(token);
          const currentTime = Date.now() / 1000; // Current time in seconds
          return decoded.exp < currentTime; // Check if the token has expired
        } catch (error) {
          console.error("Invalid token:", error);
          return true; // Treat invalid tokens as expired
        }
      }
}

export default authenticationService;