// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const inputCpf = document.querySelector('#cpf');
const inputNasc = document.querySelector('#nasc');

inputCpf.addEventListener('keypress', () => {
    let inputlength = inputCpf.value.length
    //console.log(inputlength);

    if (inputlength === 3 || inputlength === 7) {
        inputCpf.value += '.';
    } else if (inputlength === 11) {
        inputCpf.value += '-';
    }
})

inputNasc.addEventListener('keypress', () => {
    let inputlength = inputNasc.value.length

    if (inputlength === 2 || inputlength === 5) {
        inputNasc.value += '/';
    }
})

function verificarCPF(c) {
    c = c.replace(/[^\d]+/g, '');
    var verifica = c;
    var v = false;
    var i;
    s = c;
    var c = s.substr(0, 9);
    var dv = s.substr(9, 2);
    var d1 = 0;

    //if (c.length != 11 ||
    //    c == "00000000000" ||
    //    c == "11111111111" ||
    //    c == "22222222222" ||
    //    c == "33333333333" ||
    //    c == "44444444444" ||
    //    c == "55555555555" ||
    //    c == "66666666666" ||
    //    c == "77777777777" ||
    //    c == "88888888888" ||
    //    c == "99999999999")
    //    alert("CPF " + c + "  Inválido");
    //setTimeout(function () { $('#cpf').focus(); }, 1);
    //v = true;
    //return false;

   
    for (i = 0; i < 9; i++) {
        d1 += c.charAt(i) * (10 - i);
    }
    if (d1 == 0) {
        alert("CPF " + verifica + " Inválido1");
        document.getElementById("#cpf").focus();
        setTimeout(function () { $('#cpf').focus(); }, 1);
        v = true;
        return false;
    }
    d1 = 11 - (d1 % 11);
    if (d1 > 9) d1 = 0;
    if (dv.charAt(0) != d1) {
        alert("CPF " + verifica + " Inválido2");
        document.getElementById("#cpf").focus();
        setTimeout(function () { $('#cpf').focus(); }, 1);
        v = true;
        return false;
    }

    d1 *= 2;
    for (i = 0; i < 9; i++) {
        d1 += c.charAt(i) * (11 - i);
    }
    d1 = 11 - (d1 % 11);
    if (d1 > 9) d1 = 0;
    if (dv.charAt(1) != d1) {
        alert("CPF " + verifica + " Inválido3");
        document.getElementById("#cpf").focus();
        setTimeout(function () { $('#cpf').focus(); }, 1);
        v = true;
        return false;
    }
    if (!v) {
        return alert(verifica + " CPF Válido")
    }
}
function validadata() {
    const inputNasc = document.querySelector('#nasc');
    var data = document.querySelector(inputNasc); // pega o valor do input
    data = data.replace(/\//g, "-"); // substitui eventuais barras (ex. IE) "/" por hífen "-"
    var data_array = data.split("-"); // quebra a data em array

    // para o IE onde será inserido no formato dd/MM/yyyy
    if (data_array[0].length != 4) {
        data = data_array[2] + "-" + data_array[1] + "-" + data_array[0]; // remonto a data no formato yyyy/MM/dd
    }

    // comparo as datas e calculo a idade
    var hoje = new Date();
    var nasci = new Date(data);
    var idade = hoje.getFullYear() - nasci.getFullYear();
    var m = hoje.getMonth() - nasci.getMonth();
    if (m < 0 || (m === 0 && hoje.getDate() < nasci.getDate())) idade--;

    if (idade < 2) {
        alert("Pessoas menores de 18 não podem se cadastrar.");
        return false;
    }

    if (idade >= 18 && idade <= 60) {
        alert("Maior de 18, pode se cadastrar.");
        return true;
    }

    // se for maior que 60 não vai acontecer nada!
    return false;
}
function onlynumber(evt) {
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    //var regex = /^[0-9.,]+$/;
    var regex = /^[0-9]+$/;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}

//function onlynumberCpf(evt) {
//    var theEvent = evt || window.event;
//    var key = theEvent.keyCode || theEvent.which;
//    key = String.fromCharCode(key);
//    //var regex = /^[0-9.,]+$/;
//    var regex = /^[0-9.-]+$/;
//    if (!regex.test(key)) {
//        theEvent.returnValue = false;
//        if (theEvent.preventDefault) theEvent.preventDefault();
//    }
//}
