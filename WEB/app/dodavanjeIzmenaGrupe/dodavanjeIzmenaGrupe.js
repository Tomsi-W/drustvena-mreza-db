'use strict';

document.addEventListener('DOMContentLoaded', () => {
  const params = new URLSearchParams(window.location.search);
  const grupaId = params.get('grupaId');

  const form = document.getElementById('groupForm');
  const titleEl = document.getElementById('form-title');
  const idInput = document.getElementById('grupaId');
  const imeInput = document.getElementById('ime');
  const datumInput = document.getElementById('datumOsnivanja');
  const errorMsg = document.getElementById('error-msg');

  let url = 'http://localhost:3891/api/grupa';
  let method = 'POST';

  if (grupaId) {
    method = 'PUT';
    url += `/${grupaId}`;
    titleEl.textContent = 'Izmeni Grupu';

    fetch(url)
      .then(res => {
        if (!res.ok) throw new Error(`Status ${res.status}`);
        return res.json();
      })
      .then(g => {
        idInput.value = g.id;
        imeInput.value = g.ime;
        datumInput.value = g.datumOsnivanja ? g.datumOsnivanja.slice(0, 10) : ''; // csak a dátum részt
      })
      .catch(err => {
        alert('Greška pri učitavanju: ' + err.message);
        window.location.href = '../pregledGrupa/pregledGrupa.html';
      });
  }

form.addEventListener('submit', e => {
  e.preventDefault();
  const ime = imeInput.value.trim();
  const datumOsnivanja = datumInput.value;

  if (!ime || !datumOsnivanja) {
    errorMsg.textContent = 'Ime i datum osnivanja ne mogu biti prazni!';
    return;
  }

  const payload = { ime, datumOsnivanja };

  if (method === 'PUT') {
    payload.id = parseInt(grupaId);
  }

  fetch(url, {
    method,
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload)
  })
  .then(res => {
    if (!res.ok) throw new Error(`Status ${res.status}`);
    return res.json();
  })
  .then(() => {
    window.location.href = '../pregledGrupa/pregledGrupa.html';
  })
  .catch(err => {
    errorMsg.textContent = 'Greška pri čuvanju: ' + err.message;
  });
});
});
