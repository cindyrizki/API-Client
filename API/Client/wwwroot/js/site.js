// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

/*const { type } = require("jquery");*/

// Write your JavaScript code.
/*var a = document.getElementById("b2");
a.addEventListener("click", function () {
    document.getElementById("c").style.backgroundColor = "orchid";
    document.getElementById("a").innerHTML = "Coba AddEventListener!!!!";
    document.getElementById("b").style.fontStyle = "oblique";
    document.querySelector("body").style.backgroundColor = "lightblue"
});*/

/*$("#b1").click(function () {
    alert("Test JQuery!!!!!");
});*/

/*const animals = [
    { name: 'Nemo', species: 'fish', class: { name: 'invertebrata' } },
    { name: 'Simba', species: 'Cat', class: { name: 'Mamalia' } },
    { name: 'Dory', species: 'fish', class: { name: 'invertebrata' } },
    { name: 'Panther', species: 'Cat', class: { name: 'Mamalia' } },
    { name: 'Budi', species: 'Cat', class: { name: 'Mamalia' } }
]

for (let i = 0; i < animals.length; i++) {
    if (animals[i].species == 'fish') {
        animals[i].class.name = 'non mamalia';
    }
}
console.log(animals);

animals.forEach(fish);
function fish() {
    if (animals.species == 'fish') {
        animals.class.name = 'non mamalia';
    }
}
console.log(animals);

const onlyCat = [];
for (let i = 0; i < animals.length; i++) {
    if (animals[i].species == 'Cat') {
        onlyCat.push(animals[i]);
    }
}
console.log(onlyCat);*/


$.ajax({
    url: "https://swapi.dev/api/people/",
    success: function (result) {
        console.log(result.results);
        var listStarWars = "";
        $.each(result.results, function (key, val) {
            listStarWars += `<tr>
                              <th scope="row">${key+1}</th>
                              <td>${val.name}</td>
                              <td>${val.height}</td>
                              <td>${val.hair_color}</td>
                              <td>${val.skin_color}</td>
                              <td>${val.birth_year}</td>
                            </tr>`
        });
        $("#tableStar").html(listStarWars);
    }
})

$.ajax({
    url: "https://pokeapi.co/api/v2/pokemon/",
    success: function (result) {
        console.log(result.results);
        var listPoke = "";
        $.each(result.results, function (key, val) {
            listPoke += `<tr>
                              <th scope="row">${key + 1}</th>
                              <td>${val.name}</td>

                              <td><button type="button" class="btn btn-primary" data-toggle="modal" onclick="getPoke('${val.url}')" data-target="#modalPoke">
                                 Detail
                              </button></td>`
        });
        $("#tablePoke").html(listPoke);
    }
})

function getPoke(url) {
    console.log(url);
    $.ajax({
        url: url,
        success: function (result) {
            var namaPoke = "";
            var ability = "";
            var type = "";
            for (var i = 0; i < result.abilities.length; i++) {
                if (result.abilities[i].ability.name == "overgrow" || result.abilities[i].ability.name == "tinted-lens" || result.abilities[i].ability.name == "keen-eyes" || result.abilities[i].ability.name == "guts") {
                    ability += ` <span class="badge badge-pill badge-info">${result.abilities[i].ability.name}</span>  `
                }
                else if (result.abilities[i].ability.name == "chlorophyll" || result.abilities[i].ability.name == "hustle" || result.abilities[i].ability.name == "rain-dish") {
                    ability += ` <span class="badge badge-pill badge-success">${result.abilities[i].ability.name}</span> `
                }
                else if (result.abilities[i].ability.name == "blaze") {
                    ability += ` <span class="badge badge-pill badge-danger">${result.abilities[i].ability.name}</span> `
                }
                else if (result.abilities[i].ability.name == "solar-power" || result.abilities[i].ability.name == "shield-dust") {
                    ability += ` <span class="badge badge-pill badge-warning">${result.abilities[i].ability.name}</span> `
                }
                else if (result.abilities[i].ability.name == "torrent" || result.abilities[i].ability.name == "big-pecks") {
                    ability += ` <span class="badge badge-pill badge-primary">${result.abilities[i].ability.name}</span> `
                }
                else if (result.abilities[i].ability.name == "run-away" || result.abilities[i].ability.name == "swarm" || result.abilities[i].ability.name == "snipper") {
                    ability += ` <span class="badge badge-pill badge-dark">${result.abilities[i].ability.name}</span> `
                }
                else if (result.abilities[i].ability.name == "shed-skin" || result.abilities[i].ability.name == "compound-eyes") {
                    ability += ` <span class="badge badge-pill badge-secondary">${result.abilities[i].ability.name}</span> `
                }
                else if (result.abilities[i].ability.name == "tangled-feet") {
                    ability += ` <span class="badge badge-pill badge-light">${result.abilities[i].ability.name}</span> `
                }
            }
            for (var i = 0; i < result.types.length; i++) {
                if (result.types[i].type.name == "normal") {
                    type += ` <span class="badge badge-info">Normal</span>`
                }
                else if (result.types[i].type.name == "flying") {
                    type += ` <span class="badge badge-pill badge-secondary">Flying</span>`
                }
                else if (result.types[i].type.name == "bug") {
                    type += ` <span class="badge badge-pill badge-dark">Bug</span>`
                }
                else if (result.types[i].type.name == "poison") {
                    type += ` <span class="badge badge-pill badge-warning">Poison</span>`
                }
                else if (result.types[i].type.name == "water") {
                    type += ` <span class="badge badge-pill badge-primary">Water</span>`
                }
                else if (result.types[i].type.name == "fire") {
                    type += ` <span class="badge badge-pill badge-danger">Fire</span>`
                }
                else if (result.types[i].type.name == "grass") {
                    type += ` <span class="badge badge-pill badge-success">Grass</span>`
                }
            }
            namaPoke = `<img src="${result.sprites.other.dream_world.front_default}" id="poke" style="width:30%; margin-left:auto; margin-right:auto; display:block;">
                        <ul class="list-unstyled">
                            <li><strong>Name    :</strong> ${result.name}</li>
                            <li><strong>Height  :</strong> ${result.height}</li>
                            <li><strong>Weight  :</strong> ${result.weight}</li>
                            <li><strong>Abilities :</strong> ${ability}</li>
                            <li><strong>Types     :</strong> ${type}</li>
                        </ul>`
            $(".modal-body").html(namaPoke);
        }
    })
}

$.ajax({
    url: "https://pokeapi.co/api/v2/pokemon/1",
    success: function (result) {
        console.log(result);
    }
})






