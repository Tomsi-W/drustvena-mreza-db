'use strict'

class Korisnik {
    constructor(id, korisnickoIme, ime, prezime, datumRodjenja,grupeKorisnika) {
        this.id = id
        this.korisnickoIme = korisnickoIme
        this.ime = ime
        this.prezime = prezime
        this.datumRodjenja = datumRodjenja
        this.grupeKorisnika = grupeKorisnika
    }
}

function formatDate (isoDateString){
    const date= new Date (isoDateString)
    const year = date.getFullYear()
    const month = String(date.getMonth()+1).padStart(2, '0')
    const day = String(date.getDate()).padStart(2, '0')
    return `${year}-${month}-${day}`
}

document.addEventListener("DOMContentLoaded", ()=>{
    const urlParams = new URLSearchParams(window.location.search)
    const korisnikId = urlParams.get("korisnikId")

    const form = document.querySelector('#form-data')
    const errorMsg = document.querySelector('#error-msg')

    let method = "POST"
    let url = `http://localhost:3891/api/korisnik`
    let success = "upis"

    if(korisnikId){
        method = "PUT"
        url = `http://localhost:3891/api/korisnik/${korisnikId}`
        success = "izmena"


        fetch(url)
            .then(response => {
                if(!response.ok){
                    throw new Error("Greska pri ucitavanju korisnikaa. Status:" + response.status)
                }
                return response.json()
            })
            .then(korisnik => {
                document.querySelector("#korisnickoIme").value= korisnik.korisnickoIme
                document.querySelector("#ime").value = korisnik.ime
                document.querySelector("#prezime").value = korisnik.prezime
                document.querySelector("#datumRodjenja").value = formatDate(korisnik.datumRodjenja)
            })
            .catch(error=>{
                alert(error.message)
                window.location.href = "../pregledKorisnika/pregledKorisnika.html"
            })
    }
    form.addEventListener("submit", function(event){
        event.preventDefault()

        const formData = new FormData(form)
        const korisnik = {
            korisnickoIme: formData.get("korisnickoIme").trim(),
            ime: formData.get("ime").trim(),
            prezime: formData.get("prezime").trim(),
            datumRodjenja: formData.get("datumRodjenja")
        }
        if (!korisnik.korisnickoIme || !korisnik.ime || !korisnik.prezime || !korisnik.datumRodjenja) {
                errorMsg.textContent = "Sva polja su obavezna!"
                return
        }
        fetch(url, {
            method: method,
            headers:{"Content-Type": "application/json"},
            body: JSON.stringify(korisnik)
        })
        .then(response => {
            if (!response.ok){
                throw new Error("Greska pri slanju podataka. Status:" +response.status)
            }
            return response.json()
        })
        .then(()=>{
            window.location.href= `../pregledKorisnika/pregledKorisnika.html?response=${success}`
        })
        .catch(error => {
            errorMsg.textContent = error.message
        })
    })
    
})