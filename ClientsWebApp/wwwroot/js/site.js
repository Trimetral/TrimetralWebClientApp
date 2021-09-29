// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function ShowRelations() {
    document.getElementById("relTable").hidden = !document.getElementById("relTable").hidden;
}

function RandomNewClient() {
    var types = ["ООО", "ОАО", "ААА", "ЗАО", "ВАО"]
    var cnames = ["Рога", "Копыта", "Рога и копыта", "Большое", "Малое", "Белое", "Шерстяное"]
    
    document.getElementById("clName").value = types[Math.floor(Math.random() * types.length)] + " " + cnames[Math.floor(Math.random() * cnames.length)];

    var inn = Math.random();
    while (inn < 0.1) {
        inn *= 10;
    }
    document.getElementById("clInn").value = Math.floor(inn * 10000000000);

    for (var i = 0; i < 10; i++) {
        if (document.getElementById("f[" + i + "].inn") === null) break;

        inn = Math.random();
        while (inn < 0.1) {
            inn *= 10;
        }
        document.getElementById("f[" + i + "].inn").value = Math.floor(inn * 1000000000000);

        var names = ["Александр", "Владимир", "Алексей", "Сергей", "Олег", "Андрей", "Григорий", "Евгений", "Артемий"]
        var surnames = ["Белов", "Ежов", "Соколов", "Коровьев", "Неждаев", "Колаев", "Спёров", "Антонов", "Григорьев"]
        var lastnames = ["Александрович", "Владимирович", "Алексеевич", "Сергеевич", "Олегович", "Андреевич"]

        document.getElementById("f[" + i +"].name").value = surnames[Math.floor(Math.random() * surnames.length)] + " " +
            names[Math.floor(Math.random() * names.length)] + " " + lastnames[Math.floor(Math.random() * lastnames.length)];
    }
    
}

function ShowFoundersCount() {
    if (document.getElementById("Client_Type").checked === true) {
        document.getElementById("fCount").hidden = true
    }
    else {
        ClearContainer("founders")
        AddForm("founders", 0)
        document.getElementById("fCount").hidden = false
    }
}

function AddFields() {
    var number = document.getElementById("foundersCount").value;
    if (number < 1 || number > 10) {
        alert('От 1 до 10')
        return
    }

    ClearContainer("founders")
    for (i = 0; i < number; i++) {
        AddForm("founders", i)
    }
}

function ClearContainer(name) {
    var container = document.getElementById(name);
    while (container.hasChildNodes()) {
        container.removeChild(container.lastChild);
    }
}

function AddForm(name, num) {
    var mainContainer = document.getElementById(name);
    var inputContainer = document.createElement("div")
    inputContainer.id = "founder[" + num + "]"
    inputContainer.style = "margin-bottom: 70px"
    mainContainer.appendChild(inputContainer)

    var checkBox = document.createElement("input")
    checkBox.type = "checkbox"
    checkBox.id = "check[" + num + "]"
    checkBox.setAttribute("onclick", "ShowSelection(" + num + ");");
    checkBox.style = "margin-left: 40px;"
    inputContainer.appendChild(checkBox)
    inputContainer.append("Использовать существующего учредителя")

    var container = document.createElement("div")
    container.id = "manual[" + num + "]"
    inputContainer.appendChild(container)

    var formGroupInn = document.createElement("div")
    formGroupInn.classList = "form-group"
    container.appendChild(formGroupInn)

    var labelInn = document.createElement("label")
    labelInn.textContent = "ИНН Учредителя"
    labelInn.classList = "control-label"
    labelInn.for ="Founders_" + num + "__INN"
    formGroupInn.appendChild(labelInn)

    var inputInn = document.createElement("input");
    inputInn.type = "text";
    inputInn.name = "Founders[" + num + "].INN"
    inputInn.id = "f[" + num + "].inn"
    inputInn.maxLength = 12;
    inputInn.minLength = 12;
    inputInn.required = true
    inputInn.classList = "form-control"
    inputInn.setAttribute("data-val", "true")
    inputInn.setAttribute("data-val-length", "The field ИНН must be a string with a length 12.")
    inputInn.setAttribute("data-val-length-max", "12")
    inputInn.setAttribute("data-val-length-min", "12")
    inputInn.setAttribute("data-val-required", "The ИНН field is required.")
    formGroupInn.appendChild(inputInn);

    var spanInn = document.createElement("span")
    spanInn.className = 'text-danger field-validation-valid'
    spanInn.setAttribute("data-valmsg-for", "Founders[" + num + "].INN")
    spanInn.setAttribute("data-valmsg-replace", "true")
    formGroupInn.appendChild(spanInn);

    var formGroupName = document.createElement("div")
    formGroupName.classList = "form-group"
    container.appendChild(formGroupName)

    var labelName = document.createElement("label")
    labelName.textContent = "ФИО"
    labelName.classList = "control-label"
    labelName.for = "Founders_" + num + "__NameSurname"
    formGroupName.appendChild(labelName)

    var inputName = document.createElement("input");
    inputName.type = "text";
    inputName.name = "founders[" + num + "].namesurname"
    inputName.id = "f[" + num + "].name"
    inputName.className = "nameinput"
    inputName.minLength = 5;
    inputName.maxLength = 60;
    inputName.required = true
    inputName.classList = "form-control"
    inputName.setAttribute("data-val", "true")
    inputName.setAttribute("data-val-length", "The field ФИО must be a string with a minimum length of 5 and a maximum length of 60.")
    inputName.setAttribute("data-val-length-max", "60")
    inputName.setAttribute("data-val-length-min", "5")
    inputName.setAttribute("data-val-required", "The ФИО field is required.")
    formGroupName.appendChild(inputName);

    var spanName = document.createElement("span")
    spanName.className = 'text-danger field-validation-valid'
    spanName.setAttribute("data-valmsg-for", "Founders[" + num + "].NameSurname")
    spanName.setAttribute("data-valmsg-replace", "true")
    formGroupName.appendChild(spanName);

    var selectContainer = document.createElement("div")
    selectContainer.id = "selection[" + num + "]"
    selectContainer.hidden = true
    inputContainer.appendChild(selectContainer)

    var useSelected = document.createElement("input")
    useSelected.type = "text";
    useSelected.name = "selection[" + num + "].UseExistingFounder"
    useSelected.id = "selection[" + num + "].useexisting"
    useSelected.value = "false"
    useSelected.hidden = true
    selectContainer.appendChild(useSelected)

    var selection = document.getElementById("selection")
    var s_selection = selection.cloneNode(true)
    s_selection.hidden = false
    s_selection.name = "selection[" + num + "].FounderData"
    selectContainer.appendChild(s_selection)
}

function AppendBr(container, num) {
    for (var i = 0; i < num; i++) {
        container.appendChild(document.createElement("br"));
    }
}

function ShowSelection(num) {
    document.getElementById("selection[" + num + "]").hidden = !document.getElementById("selection[" + num + "]").hidden
    document.getElementById("manual[" + num + "]").hidden = !document.getElementById("manual[" + num + "]").hidden
    document.getElementById("selection[" + num + "].useexisting").value = !document.getElementById("selection[" + num + "]").hidden
    document.getElementById("f[" + num + "].inn").required = !document.getElementById("f[" + num + "].inn").required
    document.getElementById("f[" + num + "].inn").minLength = 10 * document.getElementById("f[" + num + "].inn").required
    document.getElementById("f[" + num + "].name").required = !document.getElementById("f[" + num + "].name").required
    document.getElementById("f[" + num + "].name").minLength = 10 * document.getElementById("f[" + num + "].name").required
}