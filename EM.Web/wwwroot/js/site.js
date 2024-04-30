const inputCpf = document.querySelector('#cpf');
const inputNasc = document.querySelector('#nasc');
const inputDelete = document.querySelector('#deletar');


inputCpf.addEventListener('keypress', () => {
    let inputlength = inputCpf.value.length

    if (inputlength === 3 || inputlength === 7) {
        inputCpf.value += '.';
    } else if (inputlength === 11) {
        inputCpf.value += '-';
    }
})
if (inputDelete != null) {
    inputDelete.addEventListener('keypress', () => {
        deletar();
    })
}

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
            baseCpf == "99999999999") {
            alert("CPF " + cpfOriginal + "  Inválido");
            v = true;
            setTimeout(function () { $('#cpf').focus(); }, 1);
            return false;
        }

        for (i = 0; i < 9; i++) {
            d1 += c.charAt(i) * (10 - i);
        }
        if (d1 == 0) {
            alert("CPF " + cpfOriginal + " Inválido");
            v = true;
            setTimeout(function () { $('#cpf').focus(); }, 1);
            return false;
        }
        d1 = 11 - (d1 % 11);
        if (d1 > 9) d1 = 0;
        if (dv.charAt(0) != d1) {
            alert("CPF " + cpfOriginal + " Inválido");
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
            alert("CPF " + cpfOriginal + " Inválido");

            v = true;
            setTimeout(function () { $('#cpf').focus(); }, 1);
            return false;
        }
        if (!v) {
            console.log("Cpf valido: " + cpfOriginal);
            return true;
        }
    }
}

function validadata(control) {
    var data = document.getElementById("nasc").value;
    data = data.replace(/\//g, "-");
    var data_array = data.split("-");

    if (data_array[0].length != 4) {
        data = data_array[2] + "-" + data_array[1] + "-" + data_array[0];
    }
    var hoje = new Date();
    var nasc = new Date(data);
    var idade = hoje.getFullYear() - nasc.getFullYear();
    var m = hoje.getMonth() - nasc.getMonth();
    if (m < 0 || (m === 0 && hoje.getDate() < nasc.getDate())) idade--;

    if (idade < 1 || idade > 119) {
        alert("Data inválida!");
        setTimeout(function () { $('#nasc').focus(); }, 1);
        return false;
    }

    if (idade >= 1 && idade <= 120) {
        return true;
    }
    return false;
};

function onlynumber(evt) {
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    var regex = /^[0-9]+$/;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
};

function onlynumberData(evt) {
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    var regex = /^[0-9/]+$/;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
};

function deletar(a) {
    debugger
    new swal({
        title: 'Quer mesmo deletar?',
        text: a,
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sim, tenho certeza!',
        cancelButtonText: 'Melhor não...'
    }).then((result) => {
        if (result.value == true) {
            swal(
                'Deletado!',
                'Deletado com Sucesso!',
                'success'

            )
        }
    })
};

function alerta(type, title, mensage) {
    Swal.fire({
        type: type,
        title: title,
        text: mensage,
        icon: 'warning',
        showConfirmeButton: false,
        timer: 1500,
    })
}

function validaCampoNome() {
    const nomee = document.querySelector('#nome');
    console.log(nomee.length);
    if (nomee.value.trim() == "" || nomee.length < 3) {
        Swal.fire({
            icon: 'error',
            title: 'Verifique os dados digitados!',
            showConfirmButton: false,
            timer: 1500
        })
        console.log(nomee.length);
        setTimeout(function () { $('#nome').focus(); }, 1);
        return false
    }
}

function validaCampoMatricula() {
    const matricula = document.querySelector('#matricula');
    if (matricula.value.trim() == "" || matricula.value < 1) {
        Swal.fire({
            type: type,
            title: title,
            text: mensage,
            icon: 'warning',
            showConfirmeButton: false,
            timer: 1500,
        })
        setTimeout(function () { $('#matricula').focus(); }, 1);
        return false
    }
}

