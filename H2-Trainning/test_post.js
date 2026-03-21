const http = require('http');

const options = {
  hostname: 'localhost',
  port: 8080,
  path: '/api/programs',
  method: 'POST',
  headers: {
    'Content-Type': 'application/json'
  }
};

const req = http.request(options, res => {
  let data = '';
  res.on('data', chunk => data += chunk);
  res.on('end', () => console.log('STATUS:', res.statusCode, 'DATA:', data));
});

req.on('error', e => console.error(e));

const programPayload = JSON.stringify({
  title: "Test Serialization",
  description: "Test",
  nutritionalBases: "Test",
  days: [
    {
      name: "Day 1",
      dayNumber: 1,
      isRestDay: false,
      exercises: [
        { name: "Squat", sets: 3, reps: "10", sortOrder: 0 }
      ],
      meals: []
    }
  ]
});

req.write(programPayload);
req.end();
