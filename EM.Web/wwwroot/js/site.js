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
    let cpfOriginal = c;
    if (c.length === 14) {
        c = c.replace(/[^\d]+/g, '');
        let baseCpf = c;
        let v = false;
        let i;
        s = c;
        let c = s.substr(0, 9);
        let dv = s.substr(9, 2);
        let d1 = 0;

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
            return true;
        }
    }
}

function validaCampoNome() {
    const nome = document.querySelector('#nome');
    if (nome.value.trim() == "" || nome.length < 3 && nome.length > 100) {
        Swal.fire({
            icon: 'error',
            title: 'Verifique o nome digitado!',
            showConfirmButton: false,
            timer: 1500
        })
        setTimeout(function () { $('#nome').focus(); }, 1);
        return false
    }
}

function validadataNascimento() {
    let data = document.getElementById("nasc").value;
    data = data.replace(/\//g, "-");
    let data_array = data.split("-");

    if (data_array[0].length != 4) {
        data = data_array[2] + "-" + data_array[1] + "-" + data_array[0];
    }
    let hoje = new Date();
    let nasc = new Date(data);
    let idade = hoje.getFullYear() - nasc.getFullYear();
    let m = hoje.getMonth() - nasc.getMonth();
    if (m < 0 || (m === 0 && hoje.getDate() < nasc.getDate())) idade--;

    if (idade >= 1 && idade <= 120) {
        return true;
    }

    if (isNaN(idade) || idade < 1 || idade > 119) {

        Swal.fire({
            icon: 'error',
            title: 'Verifique a data de nascimento!',
            showConfirmButton: false,
            timer: 1500
        })
        return false;
    }
}

function onlynumber(evt) {
    let theEvent = evt || window.event;
    let key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    let regex = /^[0-9]+$/;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}

function onlynumberData(evt) {
    let theEvent = evt || window.event;
    let key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    let regex = /^[0-9/]+$/;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
};

function deletar(a) {
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
}

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



