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

    if (c.length === 14) {


        c = c.replace(/[^\d]+/g, '');
        var baseCpf = c;
        var v = false;
        var i;
        s = c;
        var c = s.substr(0, 9);
        var dv = s.substr(9, 2);
        var d1 = 0;

        if (
            baseCpf == "00000000000" ||
            baseCpf == "11111111111" ||
            baseCpf == "22222222222" ||
            baseCpf == "33333333333" ||
            baseCpf == "44444444444" ||
            baseCpf == "55555555555" ||
            baseCpf == "66666666666" ||
            baseCpf == "77777777777" ||
            baseCpf == "88888888888" ||
            baseCpf == "99999999999")
            alert("CPF " + baseCpf + "  Inválido 0");
        v = true;
        setTimeout(function () { $('#cpf').focus(); }, 1);
        return false;

        for (i = 0; i < 9; i++) {
            d1 += c.charAt(i) * (10 - i);
        }
        if (d1 == 0) {
            alert("CPF " + baseCpf + " Inválido1");
            document.getElementById("#cpf").focus();
            
            v = true;
            setTimeout(function () { $('#cpf').focus(); }, 1);
            return false;
        }
        d1 = 11 - (d1 % 11);
        if (d1 > 9) d1 = 0;
        if (dv.charAt(0) != d1) {
            alert("CPF " + baseCpf + " Inválido2");
            document.getElementById("#cpf").focus();
            
            v = true;
            setTimeout(function () { $('#cpf').focus(); }, 1);
            return false;
        }

        d1 *= 2;
        for (i = 0; i < 9; i++) {
            d1 += c.charAt(i) * (11 - i);
        }
        d1 = 11 - (d1 % 11);
        if (d1 > 9) d1 = 0;
        if (dv.charAt(1) != d1) {
            alert("CPF " + baseCpf + " Inválido3");
           
            v = true;
            document.getElementById("#cpf").focus();
            return false;
        }
        //if (!v) {
        //    return alert(baseCpf + " CPF Válido")
        //}
    }
}

function validadata() {
    var data = document.getElementById("nasc").value; // pega o valor do input
    data = data.replace(/\//g, "-"); // substitui eventuais barras (ex. IE) "/" por hífen "-"
    var data_array = data.split("-"); // quebra a data em array

    // para o IE onde será inserido no formato dd/MM/yyyy
    if (data_array[0].length != 4) {
        data = data_array[2] + "-" + data_array[1] + "-" + data_array[0]; // remonto a data no formato yyyy/MM/dd
    }

    // comparo as datas e calculo a idade
    var hoje = new Date();
    var nasc = new Date(data);
    var idade = hoje.getFullYear() - nasc.getFullYear();
    var m = hoje.getMonth() - nasc.getMonth();
    if (m < 0 || (m === 0 && hoje.getDate() < nasc.getDate())) idade--;

    if (idade < 1) {
        alert("Data inválida!");
        return false;
    }

    if (idade >= 18 && idade <= 60) {
        //alert("Data valida");
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
