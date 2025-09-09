import { useNavigate } from "react-router-dom";
import { useContext } from "react";
import { AuthContext } from "../../context/AuthContext";

function AdminHomePage() {
  const navigate = useNavigate();
  const { user } = useContext(AuthContext);

  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/login");
  };

  const styles = {
    container: {
      minHeight: '100vh',
      backgroundColor: '#f8fafc',
      padding: '20px'
    },
    header: {
      backgroundColor: '#dc2626',
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
      backgroundColor: '#dc2626',
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
        <h1 style={styles.title}>Admin Dashboard</h1>
        <p style={styles.subtitle}>Welcome back, {user?.username || 'Admin'}!</p>
      </div>

      <div style={styles.grid}>
        <div style={styles.card}>
          <h3 style={styles.cardTitle}>👥 User Management</h3>
          <p style={styles.cardContent}>Manage system users and permissions</p>
        </div>

        <div style={styles.card}>
          <h3 style={styles.cardTitle}>📊 Analytics</h3>
          <p style={styles.cardContent}>View system analytics and reports</p>
        </div>

        <div style={styles.card}>
          <h3 style={styles.cardTitle}>⚙️ System Settings</h3>
          <p style={styles.cardContent}>Configure system parameters</p>
        </div>

        <div style={styles.card}>
          <h3 style={styles.cardTitle}>🔒 Security</h3>
          <p style={styles.cardContent}>Monitor security and access logs</p>
        </div>
      </div>

      <button 
        onClick={handleLogout}
        style={styles.button}
        onMouseEnter={(e) => e.target.style.backgroundColor = '#b91c1c'}
        onMouseLeave={(e) => e.target.style.backgroundColor = '#dc2626'}
      >
        Logout
      </button>
    </div>
  );
}

export default AdminHomePage;