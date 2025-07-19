import React, { useState } from 'react';
import './App.css';

function App() {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState('');
  const [itemCount, setItemCount] = useState(0);
  const [htmlReport, setHtmlReport] = useState('');

  const API_BASE = 'http://localhost:5000/api/migration';

  const loadCSV = async () => {
    setLoading(true);
    setMessage('Loading CSV...');
    try {
      const response = await fetch(`${API_BASE}/load-sample`);
      const result = await response.json();
      if (result.success) {
        setMessage(`✅ ${result.message}`);
        setItemCount(result.itemCount);
      } else {
        setMessage(`❌ ${result.message}`);
      }
    } catch (error) {
      setMessage(`❌ Error: ${error.message}`);
    }
    setLoading(false);
  };

  const showPreview = async () => {
    setLoading(true);
    setMessage('Loading preview...');
    try {
      const response = await fetch(`${API_BASE}/preview`);
      const result = await response.json();
      if (result.success) {
        setData(result.data);
        setMessage(`✅ Showing ${result.data.length} of ${result.totalCount} items`);
      } else {
        setMessage(`❌ ${result.message}`);
      }
    } catch (error) {
      setMessage(`❌ Error: ${error.message}`);
    }
    setLoading(false);
  };

  const generateReport = async () => {
    setLoading(true);
    setMessage('Generating report...');
    try {
      const response = await fetch(`${API_BASE}/report`);
      const result = await response.json();
      if (result.success) {
        setHtmlReport(result.htmlContent);
        setMessage(`✅ ${result.message}`);
      } else {
        setMessage(`❌ ${result.message}`);
      }
    } catch (error) {
      setMessage(`❌ Error: ${error.message}`);
    }
    setLoading(false);
  };

  return (
      <div className="App">
        <header className="header">
          <h1>🔄 Migration Tool</h1>
          <p>CSV to HTML Report Generator</p>
        </header>

        <div className="container">
          <div className="controls">
            <button onClick={loadCSV} disabled={loading} className="btn btn-primary">
              📁 Load CSV
            </button>
            <button onClick={showPreview} disabled={loading || itemCount === 0} className="btn btn-secondary">
              👀 Show Preview
            </button>
            <button onClick={generateReport} disabled={loading || itemCount === 0} className="btn btn-success">
              📊 Generate Report
            </button>
          </div>

          <div className="status">
            <p className={loading ? 'loading' : ''}>{message || 'Ready to load CSV data'}</p>
            {itemCount > 0 && <p className="count">📈 {itemCount} items loaded</p>}
          </div>

          {data.length > 0 && (
              <div className="data-section">
                <h3>📋 Data Preview</h3>
                <div className="table-container">
                  <table className="data-table">
                    <thead>
                    <tr>
                      <th>Item Name</th>
                      <th>Quantity</th>
                      <th>Price</th>
                      <th>Category</th>
                      <th>Total Value</th>
                    </tr>
                    </thead>
                    <tbody>
                    {data.map((item, index) => (
                        <tr key={index}>
                          <td>{item.itemName}</td>
                          <td>{item.quantity}</td>
                          <td>${item.price.toFixed(2)}</td>
                          <td>{item.category}</td>
                          <td>${item.totalValue.toFixed(2)}</td>
                        </tr>
                    ))}
                    </tbody>
                  </table>
                </div>
              </div>
          )}

          {htmlReport && (
              <div className="report-section">
                <h3>📄 Generated Report</h3>
                <div className="report-preview">
                  <iframe
                      srcDoc={htmlReport}
                      title="HTML Report"
                      width="100%"
                      height="400px"
                      style={{ border: '1px solid #ddd', borderRadius: '8px' }}
                  />
                </div>
                <button
                    className="btn btn-download"
                    onClick={() => {
                      const blob = new Blob([htmlReport], { type: 'text/html' });
                      const url = URL.createObjectURL(blob);
                      const a = document.createElement('a');
                      a.href = url;
                      a.download = 'inventory-report.html';
                      a.click();
                    }}
                >
                  💾 Download Report
                </button>
              </div>
          )}
        </div>
      </div>
  );
}

export default App;