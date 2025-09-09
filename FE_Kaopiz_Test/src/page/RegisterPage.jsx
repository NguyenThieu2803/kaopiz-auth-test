
import { useState } from "react"
import { register } from "../api/authApi"
import { useNavigate } from "react-router-dom"

function RegisterPage() {
  const [form, setForm] = useState({
    username: "",
    email: "",
    password: "",
    userType: 0,
  })
  const [isLoading, setIsLoading] = useState(false)
  const [error, setError] = useState("")
  const [success, setSuccess] = useState("")
  const [showPassword, setShowPassword] = useState(false)
  const navigate = useNavigate()

  const handleChange = (e) => {
    const { name, value, type } = e.target
    setForm({
      ...form,
      [name]: type === "select-one" ? Number.parseInt(value) : value,
    })
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setIsLoading(true)
    setError("")
    setSuccess("")

    try {
      await register(form)
      setSuccess("Registration successful! Please login.")
      setTimeout(() => navigate("/login"), 2000)
    } catch (error) {
      setError(error.response?.data?.message || "Registration failed!")
    } finally {
      setIsLoading(false)
    }
  }

  const styles = {
    container: {
      minHeight: "100vh",
      display: "flex",
      fontFamily: '"Inter", system-ui, -apple-system, sans-serif',
      overflow: "hidden",
    },
    leftPanel: {
      flex: "1",
      background: "linear-gradient(135deg, #0f172a 0%, #1e293b 25%, #334155 50%, #475569 75%, #64748b 100%)",
      display: "flex",
      flexDirection: "column",
      alignItems: "center",
      justifyContent: "center",
      padding: "60px 40px",
      position: "relative",
      color: "white",
    },
    backgroundPattern: {
      position: "absolute",
      top: 0,
      left: 0,
      right: 0,
      bottom: 0,
      backgroundImage: `
        radial-gradient(circle at 25% 25%, rgba(59, 130, 246, 0.2) 0%, transparent 50%),
        radial-gradient(circle at 75% 75%, rgba(168, 85, 247, 0.2) 0%, transparent 50%),
        radial-gradient(circle at 50% 50%, rgba(34, 197, 94, 0.1) 0%, transparent 50%)
      `,
      pointerEvents: "none",
    },
    rightPanel: {
      flex: "1",
      backgroundColor: "#ffffff",
      display: "flex",
      flexDirection: "column",
      justifyContent: "center",
      padding: "60px 80px",
      maxWidth: "600px",
      overflow: "auto",
    },
    brandingSection: {
      textAlign: "center",
      zIndex: 2,
      maxWidth: "400px",
    },
    logo: {
      width: "120px",
      height: "120px",
      background: "linear-gradient(135deg, #3b82f6 0%, #8b5cf6 100%)",
      borderRadius: "30px",
      margin: "0 auto 30px",
      display: "flex",
      alignItems: "center",
      justifyContent: "center",
      fontSize: "48px",
      color: "white",
      fontWeight: "700",
      boxShadow: "0 20px 60px rgba(59, 130, 246, 0.4)",
      position: "relative",
    },
    logoGlow: {
      position: "absolute",
      inset: "-4px",
      background: "linear-gradient(135deg, #3b82f6, #8b5cf6)",
      borderRadius: "34px",
      opacity: 0.3,
      filter: "blur(20px)",
      zIndex: -1,
    },
    brandTitle: {
      fontSize: "48px",
      fontWeight: "900",
      marginBottom: "20px",
      letterSpacing: "-0.02em",
      background: "linear-gradient(135deg, #ffffff 0%, #e2e8f0 100%)",
      backgroundClip: "text",
      WebkitBackgroundClip: "text",
      WebkitTextFillColor: "transparent",
    },
    brandSubtitle: {
      fontSize: "20px",
      fontWeight: "500",
      opacity: 0.8,
      lineHeight: 1.6,
      marginBottom: "40px",
    },
    features: {
      textAlign: "left",
      marginTop: "40px",
    },
    feature: {
      display: "flex",
      alignItems: "center",
      marginBottom: "20px",
      fontSize: "16px",
      fontWeight: "500",
      opacity: 0.9,
    },
    featureIcon: {
      marginRight: "16px",
      fontSize: "24px",
    },
    formHeader: {
      marginBottom: "40px",
    },
    title: {
      fontSize: "36px",
      fontWeight: "800",
      background: "linear-gradient(135deg, #1e293b 0%, #475569 100%)",
      backgroundClip: "text",
      WebkitBackgroundClip: "text",
      WebkitTextFillColor: "transparent",
      marginBottom: "12px",
      letterSpacing: "-0.02em",
    },
    subtitle: {
      color: "#64748b",
      fontSize: "18px",
      fontWeight: "500",
      lineHeight: 1.5,
    },
    form: {
      width: "100%",
    },
    inputGroup: {
      marginBottom: "28px",
      position: "relative",
    },
    label: {
      display: "block",
      fontSize: "16px",
      fontWeight: "600",
      color: "#1e293b",
      marginBottom: "10px",
      letterSpacing: "-0.01em",
    },
    inputWrapper: {
      position: "relative",
    },
    icon: {
      position: "absolute",
      left: "20px",
      top: "50%",
      transform: "translateY(-50%)",
      fontSize: "22px",
      zIndex: 2,
    },
    input: {
      width: "100%",
      padding: "20px 24px 20px 60px",
      border: "2px solid #e2e8f0",
      borderRadius: "16px",
      fontSize: "17px",
      outline: "none",
      transition: "all 0.3s cubic-bezier(0.4, 0, 0.2, 1)",
      backgroundColor: "#ffffff",
      fontWeight: "500",
      boxSizing: "border-box",
    },
    select: {
      width: "100%",
      padding: "20px 24px",
      border: "2px solid #e2e8f0",
      borderRadius: "16px",
      fontSize: "17px",
      outline: "none",
      transition: "all 0.3s cubic-bezier(0.4, 0, 0.2, 1)",
      backgroundColor: "#ffffff",
      fontWeight: "500",
      cursor: "pointer",
      boxSizing: "border-box",
    },
    passwordToggle: {
      position: "absolute",
      right: "20px",
      top: "50%",
      transform: "translateY(-50%)",
      background: "none",
      border: "none",
      color: "#64748b",
      cursor: "pointer",
      fontSize: "22px",
      padding: "8px",
      borderRadius: "8px",
      transition: "all 0.2s",
      zIndex: 2,
    },
    button: {
      width: "100%",
      padding: "22px 32px",
      borderRadius: "16px",
      fontWeight: "700",
      fontSize: "18px",
      color: "white",
      background: isLoading
        ? "linear-gradient(135deg, #94a3b8 0%, #64748b 100%)"
        : "linear-gradient(135deg, #3b82f6 0%, #8b5cf6 100%)",
      border: "none",
      cursor: isLoading ? "not-allowed" : "pointer",
      transition: "all 0.3s cubic-bezier(0.4, 0, 0.2, 1)",
      marginBottom: "32px",
      position: "relative",
      overflow: "hidden",
      letterSpacing: "-0.01em",
    },
    buttonGlow: {
      position: "absolute",
      inset: 0,
      background: "linear-gradient(135deg, #3b82f6, #8b5cf6)",
      opacity: 0,
      transition: "opacity 0.3s",
      borderRadius: "16px",
    },
    alert: {
      padding: "18px 24px",
      borderRadius: "16px",
      marginBottom: "28px",
      fontSize: "16px",
      fontWeight: "600",
      display: "flex",
      alignItems: "center",
      gap: "14px",
    },
    errorAlert: {
      backgroundColor: "#fef2f2",
      border: "2px solid #fecaca",
      color: "#dc2626",
    },
    successAlert: {
      backgroundColor: "#f0fdf4",
      border: "2px solid #bbf7d0",
      color: "#16a34a",
    },
    footer: {
      textAlign: "center",
      color: "#64748b",
      fontSize: "16px",
      fontWeight: "500",
    },
    link: {
      color: "#3b82f6",
      textDecoration: "none",
      fontWeight: "700",
      transition: "color 0.2s",
    },
    spinner: {
      width: "22px",
      height: "22px",
      border: "2px solid rgba(255,255,255,0.3)",
      borderTop: "2px solid white",
      borderRadius: "50%",
      animation: "spin 1s linear infinite",
      marginRight: "14px",
    },
    mobileStyles: `
      @media (max-width: 768px) {
        .container {
          flex-direction: column;
        }
        .leftPanel {
          min-height: 40vh;
          padding: 40px 20px;
        }
        .rightPanel {
          padding: 40px 20px;
        }
        .brandTitle {
          font-size: 32px;
        }
        .title {
          font-size: 28px;
        }
      }
    `,
  }

  return (
    <div style={styles.container} className="container">
      <div style={styles.leftPanel} className="leftPanel">
        <div style={styles.backgroundPattern}></div>
        <div style={styles.brandingSection}>
          <div style={styles.logo}>
            <div style={styles.logoGlow}></div>K
          </div>
          <h1 style={styles.brandTitle}>Kaopiz</h1>

          <div style={styles.features}>
            <div style={styles.feature}>
              <span style={styles.featureIcon}>🚀</span>
              <span>Innovative Solutions</span>
            </div>
            <div style={styles.feature}>
              <span style={styles.featureIcon}>🔒</span>
              <span>Secure & Reliable</span>
            </div>
            <div style={styles.feature}>
              <span style={styles.featureIcon}>⚡</span>
              <span>Lightning Fast</span>
            </div>
            <div style={styles.feature}>
              <span style={styles.featureIcon}>🌟</span>
              <span>Premium Experience</span>
            </div>
          </div>
        </div>
      </div>

      <div style={styles.rightPanel} className="rightPanel">
        <div style={styles.formHeader}>
          <h2 style={styles.title}>Create Account</h2>
          <p style={styles.subtitle}>Get started with your free account today and join thousands of satisfied users.</p>
        </div>

        {error && (
          <div style={{ ...styles.alert, ...styles.errorAlert }}>
            <span>⚠️</span>
            {error}
          </div>
        )}

        {success && (
          <div style={{ ...styles.alert, ...styles.successAlert }}>
            <span>✅</span>
            {success}
          </div>
        )}

        <form onSubmit={handleSubmit} style={styles.form}>
          <div style={styles.inputGroup}>
            <label style={styles.label}>Username</label>
            <div style={styles.inputWrapper}>
              <span style={styles.icon}>👤</span>
              <input
                name="username"
                type="text"
                placeholder="Enter your username"
                value={form.username}
                onChange={handleChange}
                required
                style={styles.input}
                onFocus={(e) => {
                  e.target.style.borderColor = "#3b82f6"
                  e.target.style.boxShadow = "0 0 0 3px rgba(59, 130, 246, 0.1)"
                }}
                onBlur={(e) => {
                  e.target.style.borderColor = "#e2e8f0"
                  e.target.style.boxShadow = "none"
                }}
              />
            </div>
          </div>

          <div style={styles.inputGroup}>
            <label style={styles.label}>Email</label>
            <div style={styles.inputWrapper}>
              <span style={styles.icon}>📧</span>
              <input
                name="email"
                type="email"
                placeholder="Enter your email"
                value={form.email}
                onChange={handleChange}
                required
                style={styles.input}
                onFocus={(e) => {
                  e.target.style.borderColor = "#3b82f6"
                  e.target.style.boxShadow = "0 0 0 3px rgba(59, 130, 246, 0.1)"
                }}
                onBlur={(e) => {
                  e.target.style.borderColor = "#e2e8f0"
                  e.target.style.boxShadow = "none"
                }}
              />
            </div>
          </div>

          <div style={styles.inputGroup}>
            <label style={styles.label}>Password</label>
            <div style={styles.inputWrapper}>
              <span style={styles.icon}>🔒</span>
              <input
                name="password"
                type={showPassword ? "text" : "password"}
                placeholder="Create a password"
                value={form.password}
                onChange={handleChange}
                required
                style={styles.input}
                onFocus={(e) => {
                  e.target.style.borderColor = "#3b82f6"
                  e.target.style.boxShadow = "0 0 0 3px rgba(59, 130, 246, 0.1)"
                }}
                onBlur={(e) => {
                  e.target.style.borderColor = "#e2e8f0"
                  e.target.style.boxShadow = "none"
                }}
              />
              <button
                type="button"
                onClick={() => setShowPassword(!showPassword)}
                style={styles.passwordToggle}
                onMouseEnter={(e) => {
                  e.target.style.backgroundColor = "#f1f5f9"
                  e.target.style.color = "#3b82f6"
                }}
                onMouseLeave={(e) => {
                  e.target.style.backgroundColor = "transparent"
                  e.target.style.color = "#64748b"
                }}
              >
                {showPassword ? "🙈" : "👁️"}
              </button>
            </div>
          </div>

          <div style={styles.inputGroup}>
            <label style={styles.label}>Account Type</label>
            <select
              name="userType"
              value={form.userType}
              onChange={handleChange}
              style={styles.select}
            >
              <option value={0}>End User</option>
              <option value={1}>Admin</option>
            </select>
          </div>

          <button
            type="submit"
            disabled={isLoading}
            style={styles.button}
            onMouseEnter={(e) => {
              if (!isLoading) {
                e.currentTarget.style.transform = "translateY(-2px)"
                e.currentTarget.style.boxShadow = "0 20px 40px -12px rgba(59, 130, 246, 0.4)"
                const buttonGlow = e.currentTarget.querySelector(".button-glow")
                if (buttonGlow) {
                  buttonGlow.style.opacity = "0.1"
                }
              }
            }}
            onMouseLeave={(e) => {
              if (!isLoading) {
                e.currentTarget.style.transform = "translateY(0)"
                e.currentTarget.style.boxShadow = "none"
                const buttonGlow = e.currentTarget.querySelector(".button-glow")
                if (buttonGlow) {
                  buttonGlow.style.opacity = "0"
                }
              }
            }}
          >
            <div className="button-glow" style={styles.buttonGlow}></div>
            {isLoading ? (
              <span
                style={{
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "center",
                  position: "relative",
                  zIndex: 1,
                }}
              >
                <span style={styles.spinner}></span>
                Creating Account...
              </span>
            ) : (
              <span style={{ position: "relative", zIndex: 1 }}>Create Account</span>
            )}
          </button>
        </form>

        <div style={styles.footer}>
          <p>
            Already have an account?{" "}
            <a
              href="/login"
              style={styles.link}
              onMouseEnter={(e) => (e.target.style.color = "#1d4ed8")}
              onMouseLeave={(e) => (e.target.style.color = "#3b82f6")}
            >
              Sign in here
            </a>
          </p>
        </div>
      </div>

      <style>{`
        @keyframes spin {
          0% { transform: rotate(0deg); }
          100% { transform: rotate(360deg); }
        }
        
        @import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800;900&display=swap');
        
        ${styles.mobileStyles}
      `}</style>
    </div>
  )
}

export default RegisterPage
