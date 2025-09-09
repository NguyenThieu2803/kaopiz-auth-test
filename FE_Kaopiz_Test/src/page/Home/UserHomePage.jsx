import { useNavigate } from "react-router-dom";
import { useContext } from "react";
import { AuthContext } from "../../context/AuthContext";

function UserHomePage() {
  const navigate = useNavigate();
  const { user } = useContext(AuthContext);

  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/login");
  };

  const styles = {
    container: {
      minHeight: '100vh',
      backgroundColor: '#f0f9ff',
      padding: '20px'
    },
    header: {
      backgroundColor: '#0284c7',
      color: 'white',
      padding: '20px',
      borderRadius: '8px',
      marginBottom: '24px'
    },
    title: {
      fontSize: '28px',
      fontWeight: 'bold',
      marginBottom: '8px'
    },
    subtitle: {
      opacity: 0.9
    },
    grid: {
      display: 'grid',
      gridTemplateColumns: 'repeat(auto-fit, minmax(250px, 1fr))',
      gap: '20px',
      marginBottom: '24px'
    },
    card: {
      backgroundColor: 'white',
      padding: '24px',
      borderRadius: '8px',
      boxShadow: '0 2px 4px rgba(0,0,0,0.1)',
      border: '1px solid #e2e8f0'
    },
    cardTitle: {
      fontSize: '18px',
      fontWeight: 'bold',
      marginBottom: '8px',
      color: '#1f2937'
    },
    cardContent: {
      color: '#6b7280'
    },
    button: {
      padding: '12px 24px',
      backgroundColor: '#0284c7',
      color: 'white',
      border: 'none',
      borderRadius: '6px',
      cursor: 'pointer',
      fontSize: '16px',
      fontWeight: '500'
    }
  };

  return (
    <div style={styles.container}>
      <div style={styles.header}>
        <h1 style={styles.title}>User Dashboard</h1>
        <p style={styles.subtitle}>Welcome, {user?.username || 'User'}!</p>
      </div>

      <div style={styles.grid}>
        <div style={styles.card}>
          <h3 style={styles.cardTitle}>📝 My Profile</h3>
          <p style={styles.cardContent}>View and edit your profile information</p>
        </div>

        <div style={styles.card}>
          <h3 style={styles.cardTitle}>📋 My Tasks</h3>
          <p style={styles.cardContent}>View your assigned tasks and projects</p>
        </div>

        <div style={styles.card}>
          <h3 style={styles.cardTitle}>📊 My Progress</h3>
          <p style={styles.cardContent}>Track your progress and achievements</p>
        </div>

        <div style={styles.card}>
          <h3 style={styles.cardTitle}>💬 Messages</h3>
          <p style={styles.cardContent}>Check your messages and notifications</p>
        </div>
      </div>

      <button 
        onClick={handleLogout}
        style={styles.button}
        onMouseEnter={(e) => e.target.style.backgroundColor = '#0369a1'}
        onMouseLeave={(e) => e.target.style.backgroundColor = '#0284c7'}
      >
        Logout
      </button>
    </div>
  );
}

export default UserHomePage;