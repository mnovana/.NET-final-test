var host = "https://localhost:";
var port = "44354/";
var projektiEndpoint = "api/projekti/";
var istrazivaciEndpoint = "api/istrazivaci/";
var loginEndpoint = "api/authentication/login/";
var registerEndpoint = "api/authentication/register/";
var editId;
var jwt_token;

// Prikaz

function showRegister() {
  document.getElementById("navigation").style.display = "none";
  document.getElementById("loginForm").style.display = "none";
  document.getElementById("registerForm").style.display = "block";
}

function showLogin() {
  document.getElementById("navigation").style.display = "none";
  document.getElementById("loginForm").style.display = "block";
  document.getElementById("registerForm").style.display = "none";
}

function showNavigation() {
  document.getElementById("navigation").style.display = "block";
  document.getElementById("loginForm").style.display = "none";
  document.getElementById("registerForm").style.display = "none";
}

function setIstrazivaci(istrazivaci) {
  let container = document.getElementById("data");
  container.innerHTML = "";

  // naslov
  let h2 = document.createElement("h2");
  h2.appendChild(document.createTextNode("Istrazivaci"));
  h2.classList.add("text-center");
  container.appendChild(h2);

  // tabela
  let table = document.createElement("table");
  table.classList.add("mx-auto");
  table.classList.add("text-center");
  table.style.width = "60%";
  let header = createIstrazivaciHeader();
  let body = document.createElement("tbody");

  // popunjavanje redova
  for (var istrazivac of istrazivaci) {
    let row = document.createElement("tr");
    row.appendChild(createTableCell(istrazivac.ime));
    row.appendChild(createTableCell(istrazivac.prezime));
    row.appendChild(createTableCell(istrazivac.godinaRodjenja));
    row.appendChild(createTableCell(istrazivac.projekatNaziv));
    if (jwt_token) {
      row.appendChild(createTableCell(istrazivac.zarada));

      // dugme delete i edit za svaki red
      var buttonEdit = document.createElement("button");
      buttonEdit.classList.add("btn");
      buttonEdit.classList.add("btn-warning");
      buttonEdit.classList.add("h-75");
      buttonEdit.name = istrazivac.id;
      buttonEdit.addEventListener("click", editIstrazivac);
      var buttonEditText = document.createTextNode("Izmeni");
      buttonEdit.appendChild(buttonEditText);
      var buttonEditCell = createTableCell("");
      buttonEditCell.appendChild(buttonEdit);
      row.appendChild(buttonEditCell);

      var buttonDelete = document.createElement("button");
      buttonDelete.classList.add("btn");
      buttonDelete.classList.add("btn-danger");
      buttonDelete.classList.add("h-75");
      buttonDelete.name = istrazivac.id;
      buttonDelete.addEventListener("click", deleteIstrazivac);
      var buttonDeleteText = document.createTextNode("Obrisi");
      buttonDelete.appendChild(buttonDeleteText);
      var buttonDeleteCell = createTableCell("");
      buttonDeleteCell.appendChild(buttonDelete);
      row.appendChild(buttonDeleteCell);
    }

    body.appendChild(row);
  }
  table.appendChild(header);
  table.appendChild(body);
  container.appendChild(table);
}

function setProjekti(projekti) {
  let select = document.getElementById("istrazivacProjekatId");
  select.innerHTML = "";

  for (var projekat of projekti) {
    let option = document.createElement("option");
    option.value = projekat.id;
    option.innerText = projekat.naziv;
    select.appendChild(option);
  }
}

function setIstrazivac(istrazivac) {
  document.getElementById("istrazivacIme").value = istrazivac.ime;
  document.getElementById("istrazivacPrezime").value = istrazivac.prezime;
  document.getElementById("istrazivacGodinaRodjenja").value = istrazivac.godinaRodjenja;
  document.getElementById("istrazivacZarada").value = istrazivac.zarada;
  // ???
  for (var option of document.getElementsByTagName("option")) {
    console.log("projekatId = " + istrazivac.projekatId + ", option.value = " + option.value);
    console.log(option.value == istrazivac.projekatId);
    if (option.value == istrazivac.projekatId) {
      option.selected = true;
    }
  }
}

function cancelEdit() {
  clearIstrazivacForm();
  document.getElementById("izmena").style.display = "none";
}

// Fetch

function loadIstrazivaci() {
  let requestUrl = host + port + istrazivaciEndpoint;
  let headers = {};

  if (jwt_token) {
    headers.Authorization = "Bearer " + jwt_token;
  }

  fetch(requestUrl, { headers: headers })
    .then((response) => {
      if (response.status === 200) {
        response.json().then(setIstrazivaci);
      } else {
        alert("Greska u loadIstrazivaci() sa kodom: " + response.status);
      }
    })
    .catch((error) => console.log(error));
}

function deleteIstrazivac() {
  let id = this.name;

  let requestUrl = host + port + istrazivaciEndpoint + id;
  let headers = {};
  if (jwt_token) {
    headers.Authorization = "Bearer " + jwt_token;
  }

  fetch(requestUrl, { method: "DELETE", headers: headers })
    .then((response) => {
      if (response.status === 204) {
        loadIstrazivaci();
      } else {
        alert("Greska u deleteIstrazivac() sa kodom: " + response.status);
      }
    })
    .catch((error) => console.log(error));
}

function editIstrazivac() {
  editId = this.name;

  let requestUrl = host + port + istrazivaciEndpoint + editId;
  let headers = {};
  if (jwt_token) {
    headers.Authorization = "Bearer " + jwt_token;
  }

  fetch(requestUrl, { method: "GET", headers: headers })
    .then((response) => {
      if (response.status === 200) {
        document.getElementById("izmena").style.display = "block";
        return loadProjekti().then(() => response.json());
      } else {
        alert("Greska u editIstrazivac() sa kodom: " + response.status);
      }
    })
    .then(setIstrazivac)
    .catch((error) => console.log(error));
}

function loadProjekti() {
  let requestUrl = host + port + projektiEndpoint;
  let headers = {};

  if (jwt_token) {
    headers.Authorization = "Bearer " + jwt_token;
  }

  return fetch(requestUrl, { headers: headers })
    .then((response) => {
      if (response.status === 200) {
        return response.json().then(setProjekti);
      } else {
        alert("Greska u loadProjekti() sa kodom: " + response.status);
      }
    })
    .catch((error) => console.log(error));
}

function submitEditForm() {
  // ucitavamo podatke iz forme i kreiramo objekat za slanje na validaciju
  let ime = document.getElementById("istrazivacIme").value;
  let prezime = document.getElementById("istrazivacPrezime").value;
  let godinaRodjenja = document.getElementById("istrazivacGodinaRodjenja").value;
  let zarada = document.getElementById("istrazivacZarada").value;
  let projekatId = document.getElementById("istrazivacProjekatId").value;
  let sendData = {
    Id: editId,
    Ime: ime,
    Prezime: prezime,
    GodinaRodjenja: godinaRodjenja,
    Zarada: zarada,
    ProjekatId: projekatId,
  };

  // validacija
  if (!istrazivacIsValid(sendData)) {
    return false;
  }

  console.log(sendData);

  let requestUrl = host + port + istrazivaciEndpoint + editId;
  let headers = { "Content-Type": "application/json" };
  if (jwt_token) {
    headers.Authorization = "Bearer " + jwt_token;
  }

  fetch(requestUrl, {
    method: "PUT",
    headers: headers,
    body: JSON.stringify(sendData),
  })
    .then((response) => {
      if (response.status === 200) {
        alert("Uspesna izmena.");
        loadIstrazivaci();
        cancelEdit();
      } else {
        alert("Greska u submitEditForm() sa kodom: " + response.status);
      }
    })
    .catch((error) => console.log(error));

  return false;
}

function searchIstrazivaci() {
  let min = document.getElementById("zaradaMin").value;
  let max = document.getElementById("zaradaMax").value;
  let sendData = {
    ZaradaMin: min,
    ZaradaMax: max,
  };

  // validacija
  if (!searchIsValid(sendData)) {
    return false;
  }

  let requestUrl = host + port + "api/pretraga/";
  let headers = { "Content-Type": "application/json" };
  if (jwt_token) {
    headers.Authorization = "Bearer " + jwt_token;
  }

  fetch(requestUrl, {
    method: "POST",
    headers: headers,
    body: JSON.stringify(sendData),
  })
    .then((response) => {
      if (response.status === 200) {
        alert("Uspesna pretraga.");
        clearPretraga();
        response.json().then(setIstrazivaci);
      } else {
        alert("Greska u searchIstrazivaci() sa kodom: " + response.status);
      }
    })
    .catch((error) => console.log(error));

  return false;
}

// Register

function registerUser() {
  let username = document.getElementById("usernameRegister").value;
  let email = document.getElementById("emailRegister").value;
  let password = document.getElementById("passwordRegister").value;
  let confirmPassword = document.getElementById("confirmPasswordRegister").value;
  let sendData = { Username: username, Email: email, Password: password, ConfirmPassword: confirmPassword };

  if (registerIsValid(sendData)) {
    let requestUrl = host + port + registerEndpoint;
    delete sendData.ConfirmPassword;
    fetch(requestUrl, { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify(sendData) })
      .then((response) => {
        if (response.status === 200) {
          alert("Uspesna registracija.");
          clearRegForm();
          showLogin();
        } else {
          alert("Greska u registerUser() sa kodom: " + response.status);
          console.log(response);
        }
      })
      .catch((error) => console.log(error));
  }
  return false;
}

// Login

function loginUser() {
  let username = document.getElementById("usernameLogin").value;
  let password = document.getElementById("passwordLogin").value;
  let passwordLoginGreska = document.getElementById("passwordLoginGreska");
  passwordLoginGreska.innerText = "";
  let sendData = { Username: username, Password: password };

  if (loginIsValid(sendData)) {
    let requestUrl = host + port + loginEndpoint;
    console.log(sendData);
    console.log(requestUrl);
    fetch(requestUrl, { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify(sendData) })
      .then((response) => {
        if (response.status === 200) {
          alert("Uspesna prijava");
          response.json().then(function (data) {
            console.log(data);
            clearLoginForm();
            document.getElementById("loginForm").style.display = "none";
            document.getElementById("userInfo").style.display = "block";
            document.getElementById("prijavljenKorisnik").innerHTML = `Prijavljen korisnik: <b>${data.username}</b>`;

            jwt_token = data.token;
            loadIstrazivaci();
            document.getElementById("pretraga").style.display = "block";
          });
        } else {
          console.log("Greska u loginUser() sa kodom: " + response.status);
          console.log(response);
          passwordLoginGreska.innerText = "Incorrect email or password.";
        }
      })
      .catch((error) => console.log(error));
  }
  return false;
}

// Logout

function logout() {
  jwt_token = undefined;
  document.getElementById("userInfo").style.display = "none";
  document.getElementById("pretraga").style.display = "none";
  document.getElementById("izmena").style.display = "none";
  document.getElementById("data").innerHTML = "";

  showNavigation();
  loadIstrazivaci();
}

// Validacije

function registerIsValid(register) {
  let isValid = true;

  if (!register.Username) {
    alert("Username required.");
    isValid = false;
  } else if (register.Username.length > 256) {
    alert("Maximum username length is 256.");
    isValid = false;
  }

  if (!register.Email) {
    alert("Email required.");
    isValid = false;
  } else if (register.Email.length > 256) {
    alert("Maximum email length is 256.");
    isValid = false;
  }

  if (!register.Password) {
    alert("Password required.");
    isValid = false;
  } else if (register.Password.length > 256) {
    alert("Maximum password length is 256.");
    isValid = false;
  }

  if (!register.ConfirmPassword) {
    alert("Confirmed password required.");
    isValid = false;
  } else if (register.Password !== register.ConfirmPassword) {
    alert("Confirmed password doesn't match.");
    isValid = false;
  }

  return isValid;
}

function loginIsValid(login) {
  let isValid = true;

  if (!login.Username) {
    alert("Username required.");
    isValid = false;
  } else if (login.Username.length > 256) {
    alert("Maximum username length is 256.");
    isValid = false;
  }

  if (!login.Password) {
    alert("Password required.");
    isValid = false;
  } else if (login.Password.length > 256) {
    alert("Maximum password length is 256.");
    isValid = false;
  }

  return isValid;
}

function searchIsValid(search) {
  let isValid = true;

  if (!search.ZaradaMin) {
    alert("Min salary required.");
    isValid = false;
  } else if (Number(search.ZaradaMin) < 10000 || Number(search.ZaradaMin) > 500000) {
    alert("Salary must be in range 10000-500000.");
    isValid = false;
  }

  if (!search.ZaradaMax) {
    alert("Max salary required.");
    isValid = false;
  } else if (Number(search.ZaradaMax) < 1 || Number(search.ZaradaMax) > 250000) {
    alert("Salary must be in range 10000-500000.");
    isValid = false;
  } else if (isValid) {
    if (Number(search.ZaradaMin) > Number(search.ZaradaMax)) {
      alert("Max must be greater than min.");
      isValid = false;
    }
  }

  return isValid;
}

function istrazivacIsValid(istrazivac) {
  let isValid = true;

  if (!istrazivac.Ime) {
    alert("Name required.");
    isValid = false;
  } else if (istrazivac.Ime.length > 150) {
    alert("Maximum name length is 50.");
    isValid = false;
  }

  if (!istrazivac.Prezime) {
    alert("OS required.");
    isValid = false;
  } else if (istrazivac.Prezime.length > 80) {
    alert("Maximum surname length is 80.");
    isValid = false;
  }

  if (!istrazivac.GodinaRodjenja) {
    alert("Year required.");
    isValid = false;
  } else if (Number(istrazivac.GodinaRodjenja) < 1900 || Number(istrazivac.GodinaRodjenja) > 2024) {
    alert("Year must be in range 1900-2024.");
    isValid = false;
  }

  if (!istrazivac.Zarada) {
    alert("Salary required.");
    isValid = false;
  } else if (Number(istrazivac.Zarada) < 10000 || Number(istrazivac.Zarada) > 500000) {
    alert("Salary must be in range 10000-500000.");
    isValid = false;
  }

  return isValid;
}

// Pomocne funkcije

function clearLoginForm() {
  document.getElementById("usernameLogin").value = "";
  document.getElementById("passwordLogin").value = "";
}

function clearRegForm() {
  document.getElementById("usernameRegister").value = "";
  document.getElementById("passwordRegister").value = "";
  document.getElementById("emailRegister").value = "";
  document.getElementById("confirmPasswordRegister").value = "";
}

function clearPretraga() {
  document.getElementById("zaradaMin").value = "";
  document.getElementById("zaradaMax").value = "";
}

function clearIstrazivacForm() {
  document.getElementById("istrazivacIme").value = "";
  document.getElementById("istrazivacPrezime").value = "";
  document.getElementById("istrazivacGodinaRodjenja").value = "";
  document.getElementById("istrazivacZarada").value = "";
}

function createIstrazivaciHeader() {
  let head = document.createElement("thead");
  let row = document.createElement("tr");
  row.style.backgroundColor = "lightblue";
  row.style.borderBottom = "2px solid black";

  row.appendChild(createHeaderTableCell("Ime"));
  row.appendChild(createHeaderTableCell("Prezime"));
  row.appendChild(createHeaderTableCell("Godina rodjenja"));
  row.appendChild(createHeaderTableCell("Projekat"));

  if (jwt_token) {
    row.appendChild(createHeaderTableCell("Zarada (din)"));
    row.appendChild(createHeaderTableCell("Izmena"));
    row.appendChild(createHeaderTableCell("Brisanje"));
  }

  head.appendChild(row);

  return head;
}

function createHeaderTableCell(text) {
  var cell = document.createElement("th");
  var cellText = document.createTextNode(text);
  cell.appendChild(cellText);
  cell.style.height = "40px";

  return cell;
}

function createTableCell(text) {
  var cell = document.createElement("td");
  var cellText = document.createTextNode(text);
  cell.appendChild(cellText);
  cell.style.border = "1px solid black";
  cell.style.height = "50px";
  return cell;
}
