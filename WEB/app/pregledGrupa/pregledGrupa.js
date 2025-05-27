'use strict'

// Model klase
class Grupa {
    constructor(id, ime) {
        this.id = id
        this.ime = ime
    }
}

// Globális változók az új részhez
const detaljiDivId = 'detaljiGrupe'
let detaljiDiv, clanoviTabela, ostaliTabela

// Inicijalizacija kada se stranica ucita
function initializeGrupe() {
    loadGrupe()

    // Dodaj Grupu gomb kezelése
    const dodajBtn = document.getElementById('dodajGrupuBtn')
    if (dodajBtn) {
        dodajBtn.addEventListener('click', () => {
            window.location.href = '../dodavanjeIzmenaGrupe/dodavanjeIzmenaGrupe.html'
        })
    }

    // Az új rész elemeinek elérése
    detaljiDiv = document.getElementById(detaljiDivId)
    clanoviTabela = document.getElementById('clanoviTabela')
    ostaliTabela = document.getElementById('ostaliTabela')
}

// Ucitavanje svih grupa sa servera
function loadGrupe() {
    fetch('http://localhost:3891/api/grupa') 
        .then(response => {
            if (!response.ok) {
                throw new Error("Greška prilikom zahteva: " + response.status)
            }
            return response.json()
        })
        .then(grupe => {
            createGroupTable(grupe)
        })
        .catch(error => {
            console.error("Greška:", error.message)
            alert("Došlo je do greške prilikom učitavanja grupa.")
        })
}

// Kreiranje HTML tabele sa ucitanim grupama
function createGroupTable(grupe) {
    const container = document.querySelector("#grupaLista")
    container.innerHTML = `
        <table class="user-data">
            <thead class="user-data-head">
                <tr>
                    <th>ID</th>
                    <th>Naziv</th>
                    <th>Opcije</th>
                </tr>
            </thead>
            <tbody id="group-data-body"></tbody>
        </table>
    `
    const tbody = container.querySelector("#group-data-body")

    for (let grupa of grupe) {
        const row = document.createElement("tr")
        row.innerHTML = `
            <td>${grupa.id}</td>
            <td>${grupa.ime}</td>
            <td>
              <button class="deleteGroupBtn" data-id="${grupa.id}">Obriši</button>
            </td>
        `

        // Brisanje
        const deleteBtn = row.querySelector(".deleteGroupBtn")
        deleteBtn.addEventListener("click", (e) => {
            e.stopPropagation() // ne aktiválja a sor kattintás eseményt
            deleteGrupa(grupa.id)
        })

        // Sorra kattintva betöltjük a tagokat és nem tagokat
        row.addEventListener("click", () => {
            ucitajClanoveIPripadneListe(grupa.id)
        })

        tbody.appendChild(row)
    }
}

// Slanje delete zahteva
function deleteGrupa(id) {
    fetch(`http://localhost:3891/api/grupa/${id}`, {
        method: 'DELETE'
    })
    .then(res => {
        if (res.ok) {
            loadGrupe()
            // Ha a részletek meg vannak nyitva, rejtse el őket törlés után
            if (detaljiDiv) {
                detaljiDiv.style.display = 'none'
            }
        } else {
            alert("Brisanje grupe nije uspelo.")
        }
    })
}

// Új rész: tagok és nem tagok betöltése, megjelenítése
async function ucitajClanoveIPripadneListe(grupaId) {
    try {
        if (!detaljiDiv) return
        detaljiDiv.style.display = 'block'

        const clanoviRes = await fetch(`http://localhost:3891/api/grupa/${grupaId}/korisnik`)
        if (!clanoviRes.ok) throw new Error("Greška pri učitavanju članova grupe")
        const clanovi = await clanoviRes.json()

        const sviRes = await fetch('http://localhost:3891/api/korisnik')
        if (!sviRes.ok) throw new Error("Greška pri učitavanju korisnika")
        const svi = await sviRes.json()

        const clanoviIds = new Set(clanovi.map(k => k.id))
        const ostali = svi.filter(k => !clanoviIds.has(k.id))

        // Táblázat feltöltése
        clanoviTabela.innerHTML = `
            <thead><tr><th>Ime</th><th>Prezime</th><th>Akcija</th></tr></thead>
            <tbody>
            ${clanovi.map(k => `
                <tr>
                    <td>${k.ime}</td>
                    <td>${k.prezime}</td>
                    <td><button onclick="ukloniClana(${grupaId}, ${k.id})">Ukloni</button></td>
                </tr>
            `).join('')}
            </tbody>
        `

        ostaliTabela.innerHTML = `
            <thead><tr><th>Ime</th><th>Prezime</th><th>Akcija</th></tr></thead>
            <tbody>
            ${ostali.map(k => `
                <tr>
                    <td>${k.ime}</td>
                    <td>${k.prezime}</td>
                    <td><button onclick="dodajClana(${grupaId}, ${k.id})">Dodaj</button></td>
                </tr>
            `).join('')}
            </tbody>
        `
    } catch (e) {
        alert("Greška pri učitavanju detalja: " + e.message)
    }
}

// Tag eltávolítása a csoportból
window.ukloniClana = async function(grupaId, korisnikId) {
    const res = await fetch(`http://localhost:3891/api/grupa/${grupaId}/korisnici/${korisnikId}`, {
        method: 'DELETE'
    })
    if (res.ok) {
        ucitajClanoveIPripadneListe(grupaId)
    } else {
        alert("Greška pri uklanjanju člana")
    }
}

// Tag hozzáadása a csoporthoz
window.dodajClana = async function(grupaId, korisnikId) {
    const res = await fetch(`http://localhost:3891/api/grupa/${grupaId}/korisnici/${korisnikId}`, {
        method: 'PUT'
    })
    if (res.ok) {
        ucitajClanoveIPripadneListe(grupaId)
    } else {
        alert("Greška pri dodavanju člana")
    }
}

// Pokretanje prilikom ucitavanja stranice
document.addEventListener("DOMContentLoaded", initializeGrupe)
