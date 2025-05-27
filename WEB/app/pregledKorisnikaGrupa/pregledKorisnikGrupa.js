'use strict'

class Korisnik{
    constructor(id, korisnickoIme, ime, prezime, datumRodjenja, grupaKorisnika){
        this.id = id
        this.korisnickoIme = korisnickoIme
        this.ime = ime
        this.prezime = prezime
        this.datumRodjenja = datumRodjenja
        this.grupaKorisnika = grupaKorisnika
    }
}

class Grupa{
    constructor(id, ime, datumOsnivanja){
        this.id = id
        this.ime= ime
        this.datumOsnivanja= datumOsnivanja
    }
}

const urlParams = new URLSearchParams(window.location.search)
const grupaId = urlParams.get('grupaId')
const grupaIme = urlParams.get('grupaIme')

function formatDate(isoDateString) {
    const date = new Date(isoDateString)
    return date.toLocaleDateString('sr-RS')
}

function loadKorisnikGrupe(){
    fetch(`http://localhost:3891/api/grupa/${id}/korisnik`)
        .then(response => {
            if(!response.ok){
                throw new Error("Greska prilikom dohvatanja korisnika grupe.")
            }
            return response.json()
        })
        .then(korisnici=> {
            prikaziKorisnike(korisnici)
        })
        .catch(error=> {
            console.log("Doslo je do greske:", error.message)
            alert("Nesupesno ucitavanje korisnika.")
        })
}

function prikaziKorisnike(korisnici){
    const naslov = document.querySelector('#naziv-grupe')
    naslov.textContent = grupaIme

    const container = document.querySelector('.main-content')

    if(!korisnici || korisnici.length === 0){
        container.innerHTML = `<p>U grupi nema nijednog korisnika</p>`
        return
    }
    container.innerHTML = `
        <table class="user-data">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Korisničko ime</th>
                    <th>Ime</th>
                    <th>Prezime</th>
                    <th>Datum rođenja</th>
                </tr>
            </thead>
            <tbody id="user-data-body"></tbody>
        </table>
    `

    const tbody = document.getElementById("user-data-body")

    korisnici.forEach(korisnik => {
        const row = document.createElement("tr")
        row.innerHTML = `
        <td>${korisnik.id}</td>
            <td>${korisnik.korisnickoIme}</td>
            <td>${korisnik.ime}</td>
            <td>${korisnik.prezime}</td>
            <td>${formatDate(korisnik.datumRodjenja)}</td>
        `
        tbody.appendChild(row)
    });
}

document.addEventListener('DOMContentLoaded', loadKorisnikGrupe)