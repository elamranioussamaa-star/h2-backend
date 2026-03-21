fetch('http://localhost:8080/api/programs', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    title: "Test Serialization",
    description: "Test",
    nutritionalBases: "Test",
    days: [
      {
        name: "Day 1",
        dayNumber: 1,
        isRestDay: false,
        exercises: [],
        meals: []
      }
    ]
  })
}).then(async r => {
  console.log('STATUS:', r.status);
  console.log('BODY:', await r.text());
}).catch(console.error);
