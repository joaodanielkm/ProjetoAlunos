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
    var cpfOriginal = c;
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
            alert("CPF " + cpfOriginal + "  Inválido 0");
        console.log(cpfOriginal);
        v = true;
        setTimeout(function () { $('#cpf').focus(); }, 1);
        return false;

        for (i = 0; i < 9; i++) {
            d1 += c.charAt(i) * (10 - i);
        }
        if (d1 == 0) {
            alert("CPF " + cpfOriginal + " Inválido1");
            //document.getElementById("#cpf").focus();
            console.log(cpfOriginal);
            v = true;
            new setTimeout(function () { $('#cpf').focus(); }, 1);
            return false;
        }
        d1 = 11 - (d1 % 11);
        if (d1 > 9) d1 = 0;
        if (dv.charAt(0) != d1) {
            alert("CPF " + cpfOriginal + " Inválido2");
            //document.getElementById("#cpf").focus();
            console.log(cpfOriginal);
            v = true;
            new setTimeout(function () { $('#cpf').focus(); }, 1);
            return false;
        }

        d1 *= 2;
        for (i = 0; i < 9; i++) {
            d1 += c.charAt(i) * (11 - i);
        }
        d1 = 11 - (d1 % 11);
        if (d1 > 9) d1 = 0;
        if (dv.charAt(1) != d1) {
            console.log(cpfOriginal);
            alert("CPF " + cpfOriginal + " Inválido3");
           
            v = true;
            new setTimeout(function () { $('#cpf').focus(); }, 1);
            return false;
        }
        if (!v) {
            return alert(cpfOriginal + " CPF Válido")
            console.log(cpfOriginal);
            return true;
        }
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

    if (idade < 1 || idade > 119 ) {
        alert("Data inválida!");
        setTimeout(function () { $('#nasc').focus(); }, 1);
        console.log(idade);
        return false;
    }

    if (idade >= 1 && idade <= 120) {
        //alert("Data valida");
        console.log(idade);
        return true;
    }

    // se for maior que 120 não vai acontecer nada!
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

//document.getElementById("pesq").disabled = true;

////cria um event listener que escuta mudanças no input
//document.getElementById("SearchString").addEventListener("SearchString", function (event) {

//    //busca conteúdo do input
//    var conteudo = document.getElementById("SearchString").value;

//    //valida conteudo do input 
//    if (conteudo !== null && conteudo !== '') {
//        //habilita o botão
//        document.getElementById("pesq").disabled = false;
//    } else {
//        //desabilita o botão se o conteúdo do input ficar em branco
//        document.getElementById("pesq").disabled = true;
//    }
//});