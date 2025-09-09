import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import { AuthProvider, AuthContext } from "./context/AuthContext";
import ProtectedRoute from "./components/ProtectedRoute";
import LoginPage from "./page/LoginPage";
import RegisterPage from "./page/RegisterPage";
import AdminHomePage from "./page/Home/AdminHomePage";
import UserHomePage from "./page/Home/UserHomePage";
import { useContext } from "react";

function RoleBasedHome() {
  const { user } = useContext(AuthContext);
  
  // userType: 0 = End User, 1 = Admin
  if (user?.userType === 1) {
    return <AdminHomePage />;
  } else {
    return <UserHomePage />;
  }
}

function App() {
  return (
    <AuthProvider>
      <Router>
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route
            path="/home"
            element={
              <ProtectedRoute>
                <RoleBasedHome />
              </ProtectedRoute>
            }
          />
          <Route path="/" element={<Navigate to="/home" />} />
        </Routes>
      </Router>
    </AuthProvider>
  );
}

export default App;