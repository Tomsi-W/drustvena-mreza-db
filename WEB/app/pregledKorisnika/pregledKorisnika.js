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

const urlParams = new URLSearchParams(window.location.search)
const response =urlParams.get('response')

function initializeKorisnike(){
    loadKorisnike()
    showSuccess()
}

function loadKorisnike(){
    console.log("Pokrećem funkciju loadKorisnike()...")

    fetch('http://localhost:3891/api/korisnik')
        .then(response => {
            if(!response.ok){
                throw new Error('Zahtev odbijen. Status:' + response.status)
            }
            return response.json()
        })
        .then(korisnici => {
            createDataTable(korisnici)
            saveLocalStorage(korisnici)
        })
        .catch(error=> {
            console.error('Error:', error.message)
            alert('Doslo je do greske prilikom ucitavanja korisnika.')
        });
}

function createDataTable(korisnici){
    let container = document.querySelector('.main-content')
    container.innerHTML = `
        <table class="user-data">
            <thead class="user-data-head">
                <tr>
                    <th>Id</th> 
                    <th>Korisnicko ime</th> 
                    <th>Ime</th> 
                    <th>Prezime</th> 
                    <th>Datum rodjenja</th>
                    <th>Opcije</th>
                </tr>
            </thead>
            <tbody id="user-data-body"></tbody>
        </table>
        `
    const tbody = container.querySelector("#user-data-body")

    for(let korisnik of korisnici){
        const row = document.createElement("tr")
        row.innerHTML = `
           <td>${korisnik.id}</td>
            <td>${korisnik.korisnickoIme}</td>
            <td>${korisnik.ime}</td>
            <td>${korisnik.prezime}</td>
            <td>${formatDate(korisnik.datumRodjenja)}</td>
            <td><button class="izmeniKorisnikaBtn">Izmeni podatke</button></td> 
            `
        let button = row.querySelector(".izmeniKorisnikaBtn")
        button.addEventListener("click", ()=>{
            window.location.href = `../dodavanjeIzmenaKorisnika/dodavanjeIzmenaKorisnika.html?korisnikId=${korisnik.id}`
        })

        tbody.appendChild(row)
    }
}
function formatDate(isoDateString){
    const date = new Date(isoDateString)
    return date.toLocaleDateString('sr-RS')
}
function saveLocalStorage(korisnici){
    const korisniciJson = JSON.stringify(korisnici)
    localStorage.setItem("korisnici", korisniciJson)
}

function showSuccess(){
    const successMsg = document.querySelector("#potvrdna-poruka")
    if(!successMsg) return

    if(response ==="upis"){
        successMsg.textContent = "Korisnik je uspesno dodat"
    }
    else if(response ==="izmena"){
        successMsg.textContent = "Korisnik je uspesno izmenjen."
    }
    else{
        return
    }
    
    successMsg.style.opacity = "1"
    successMsg.style.color = "green"
    successMsg.style.fontWeight = "bold"

    setTimeout(() =>{
        successMsg.style.opacity = "0"
    }, 3000)
}

const dodajBtn = document.querySelector("#dodajKorisnikaBtn")
if(dodajBtn){
    dodajBtn.addEventListener("click", function(){
        window.location.href = `../dodavanjeIzmenaKorisnika/dodavanjeIzmenaKorisnika.html`
    })
}

document.addEventListener("DOMContentLoaded", initializeKorisnike)
