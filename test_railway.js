fetch('https://faithful-mindfulness-production.up.railway.app/api/programs/1')
  .then(async r => {
    console.log('STATUS:', r.status);
    console.log('BODY:', await r.text());
  })
  .catch(console.error);
