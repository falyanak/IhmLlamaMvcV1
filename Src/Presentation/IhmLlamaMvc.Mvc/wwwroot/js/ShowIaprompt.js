window.onload = function () {
    //    alert("onload")
    hideBusyIndicator();
    // Écouteur d'événement pour le bouton de réinitialisation
    document.getElementById("resetButton").addEventListener("click", function () { resetConversation(); });
    document.getElementById("searchForm").addEventListener("submit", function (event) { postData(event); });
    document.getElementById("ModeleId").addEventListener("change", function () { resetConversation(); });

    // ajout évènement click aux boutons de l'historique
    setTimeout(() => {
        document.querySelectorAll(".deleteChat").forEach(i => i.addEventListener(
            "click", () => deleteChat()));

        document.querySelectorAll(".openChat").forEach(i => i.addEventListener(
            "click", () => openChat()));


    }, 500);

    const textareas = document.querySelectorAll("textarea");
    //  var test = document.querySelectorAll('input[value][type="checkbox"]:not([value=""])

    const questions = document.querySelectorAll(".questionclass");
    //  console.log(questions);

    const reponses = document.querySelectorAll(".reponseclass");
    //  console.log(reponses);

    document.getElementById("Question").addEventListener("keypress", function (event) {
        if (event.key === 'Enter') {
            event.preventDefault();
            document.getElementById("searchForm").requestSubmit();
        }
    });

    textareas.forEach(function (i) {
        i.addEventListener('input', function () {
            //  console.log("on input");
            setDynamicHeight(this);
        });
    });


    clearSessionStorage();
    storeObjectInSessionStorage(QUESTION_VALEUR_MAX_ID, 0);
    storeObjectInSessionStorage(REPONSE_VALEUR_MAX_ID, 0);

    //const sidebar = document.querySelector('#sidebar');
    //const showSidebar = document.querySelector('#show-sidebar');
    //const closeSidebar = document.querySelector('#close-sidebar');

    //showSidebar.onclick = () => {
    //    sidebar.classList.toggle('collapsed');
    //};

    //closeSidebar.onclick = () => {
    //    sidebar.classList.toggle('collapsed');
    //};

    // cacher l'indicateur de chargement
    hideBusyIndicator();

    CreerListeHistorique();

    Question.focus()
};

// envoyer les données du formulaire au serveur
async function postData(event) {

    event.preventDefault();

    const question = document.getElementById("Question").value;
    if (question.length == 0) {
        alert("Une question doit être saisie !!");
        return false;
    }

    document.getElementById("rechercher").disabled = true;

    // empêcher la saisie pendant la soumission du formulaire
    preventInputData(true);

    // afficher le sablier
    showBusyIndicator();

    const f = document.getElementById("searchForm");
    const formData = new FormData(f);

    const modeleId = document.getElementById("ModeleId").value;
    formData.append("ModeleId", modeleId);
    // console.log(formData);

    const url = $('#mapconversation').data('url-soumettre-formulaire-link');

    var errorResponse;

    try {
        const response = await fetch(url, {
            method: "POST",
            body: formData
        });

        const result = await response.json();
        // console.log("Success:", result);

        if (!response.ok) {
            errorResponse = result;
            throw new Error("Une erreur s'est produite !");
        }

        //      alert(result);

        // rendre l'IHM disponible pour la saisie
        preventInputData(false);

        // cacher l'indicateur de chargement
        hideBusyIndicator();

        // afficher la question
        showQuestion();

        // afficher la réponse
        showResponse(result);

        //    document.getElementsByClassName("left-sidebar-grid").style.gridTemplateRows = "auto";
        document.getElementById("Question").value = "";

    } catch (error) {
        console.log("Error:", error);

        //extraire les erreurs au format object{ code: string, message : string }
        if (errorResponse && errorResponse.errors) {
            const errorList = errorResponse.errors;
            showAlert("Une erreur est survenue", "message", "danger", errorList,
                { autoClose: false })
        }
        else {
            const errorList = [{ code: "Une erreur s'est produite", message: error }];
            showAlert("Une erreur est survenue", "message", "danger", errorList,
                { autoClose: false })
        }
    }
}

function errorFunc() {
    alert('error');
}

function resetConversation() {
    // Vide la zone de texte
    document.getElementById("Question").value = "";
    document.getElementById("Question").style.height = "41px";

    // effacer les questions et réponses
    const parent = document.getElementById("showQuestionAnswer");
    while (parent.firstChild) {
        parent.firstChild.remove()
    }

    document.getElementById("Question").focus();
}

function preventInputData(actionValue = false) {
    //  console.log(`accessibilite IHM = ${actionValue}`);

    document.getElementById("Question").readOnly = actionValue;
    document.getElementById("rechercher").readOnly = actionValue;
}

function setDynamicHeight(element) {
    element.style.height = 0; // set the height to 0 in case of it has to be shrinked

    //  console.log(`element.scrollHeight = ${element.scrollHeight} pour élément ${element.name}`)

    // temporisation pour la mise à jour de la hauteur
    setTimeout(function () {
        element.style.height = element.scrollHeight + 'px'; // set the dynamic height
    }, 50);


}


function getIconIaPath() {
    const LLAMA_ID = "1";
    const PHI_ID = "2";
    const modelId = document.getElementById("ModeleId").value;

    //  alert(`modelId = ${modelId}`);

    switch (modelId) {
        case LLAMA_ID:
            return $('#mapconversation').data('icon-path-ollama');

        case PHI_ID:
            return $('#mapconversation').data('icon-path-phi');

        default: return "Icone IA non trouvé !"
    }
}

function showResponse(data) {

    const nextId = getReponseMaxId();

    const iconIaPath = getIconIaPath();

    //   alert(`iconIaPath = ${iconIaPath}`);

    createTextareaAnswer(nextId, data, iconIaPath);

    const id = `reponseId-${nextId}`;

    const answer = document.getElementById(id);

    //    alert(answer.name);
    answer.value = data;

    setDynamicHeight(answer);
}

function getReponseMaxId() {
    let id = restoreObjectInSessionStorage(REPONSE_VALEUR_MAX_ID);
    // console.log(`getReponseMaxId = ${id}`);

    id = parseInt(id, 10) + 1;

    //  console.log(`getReponseMaxId après incrément = ${id}`);

    // stocker la nouvelle valeur
    storeNewMaxId(REPONSE_VALEUR_MAX_ID, id);

    //  console.log(`stockage en session de REPONSE_VALEUR_MAX_ID = ${id}`);
    return id;
}


function createTextareaAnswer(nextId, content, iconIaPath) {
    let myDiv = document.createElement('div');
    myDiv.id = `divreponseid-${nextId}`;
    myDiv.className = 'container';

    let myImg = document.createElement('img');
    //  const iconPath = $('#mapconversation').data('icon-path-ollama');
    myImg.src = iconIaPath;
    myImg.height = "32";
    myImg.width = "32";

    myDiv.appendChild(myImg);

    var input = document.createElement('textarea');
    input.name = `reponseId-${nextId}`;
    input.id = `reponseId-${nextId}`;

    input.readOnly = true;
    input.className = 'reponseclass';
    input.innerText = content;

    myDiv.appendChild(input);

    const parent = document.querySelector("#showQuestionAnswer");

    parent.appendChild(myDiv);
}


function createTextareaQuestion(nextId, content) {
    const initialesUser = $('#mapconversation').data('url-initiales-user');

    let myDiv = document.createElement('div');
    myDiv.id = `divquestionid-${nextId}`;
    myDiv.className = 'container';
    myDiv.innerText = `${initialesUser}`;

    //let mySpan = document.createElement('span');
    //mySpan.innerText = `${initialesUser}`;
    //mySpan.width = "50px";
    //mySpan.display="inline-block";

    //myDiv.appendChild(mySpan);

    let input = document.createElement('textarea');
    input.name = `questionId-${nextId}`;
    input.id = `questionId-${nextId}`;
    input.readOnly = true;
    input.className = 'questionclass';
    input.innerText = content;

    myDiv.appendChild(input);

    const parent = document.querySelector("#showQuestionAnswer");

    parent.appendChild(myDiv);
}

function getQuestionMaxId() {
    let id = restoreObjectInSessionStorage(QUESTION_VALEUR_MAX_ID);
    //   console.log(`getQuestionMaxId = ${id}`);

    id = parseInt(id, 10) + 1;
    //   console.log(`getQuestionMaxId après incrément = ${id}`);

    // stocker la nouvelle valeur
    storeNewMaxId(QUESTION_VALEUR_MAX_ID, id);

    //   console.log(`stockage en session de QUESTION_VALEUR_MAX_ID = ${id}`);

    return id;
}

function storeNewMaxId(key, valueId) {
    // stocker la nouvelle valeur
    storeObjectInSessionStorage(key, valueId);
}

function showQuestion() {

    const nextId = getQuestionMaxId();

    const data = document.getElementById("Question").value;

    const initiales = document.getElementById("InitialesAgent").value;
    const dataWithPrefix = `${initiales} : ${data}`;

    createTextareaQuestion(nextId, dataWithPrefix);

    const id = `questionId-${nextId}`;

    const question = document.getElementById(id);

    //   alert(`question = ${question}`);

    setDynamicHeight(question);

    // recopier question dans nouveau textArea
    question.value = data;
}

//function CreerListeHistorique() {
//    var grabList = document.getElementById('listeHistorique');

//    for (let i = 0; i < 6; i++) {
//        text = `INFO ${i}`
//        let entry = document.createElement('li');
//        entry.id = `li-id-${i}`;
//        entry.className = "itemListAvecBoutton";
//        entry.appendChild(document.createTextNode(text));

//        /*Add a button to each LI */
//        let button2 = document.createElement('button');

//        let myImg2 = document.createElement('img');
//        const iconPath2 = $('#mapconversation').data('icon-open-chat');
//        myImg2.src = iconPath2;
//        myImg2.height = "16";
//        myImg2.width = "16";

//        button2.appendChild(myImg2);

//        entry.appendChild(button2);

//        let button1 = document.createElement('button');

//        let myImg1 = document.createElement('img');
//        const iconPath1 = $('#mapconversation').data('icon-delete');
//        myImg1.src = iconPath1;
//        myImg1.height = "16";
//        myImg1.width = "16";

//        button1.appendChild(myImg1);

//        entry.appendChild(button1);


//        grabList.appendChild(entry);
//    }
//}
function CreerListeHistorique() {
    var grabList = document.getElementById('listeHistorique');

    for (let i = 0; i < 6; i++) {
        const text = `L'albatros de charles Baudelaire`
        let div0 = document.createElement('div');
        div0.className = "containerHistorique";
        div0.appendChild(document.createTextNode(text));

        let div1 = document.createElement('div');
        let button2 = document.createElement('button');
        button2.title = "Ouvrir le chat";
        //    button2.className = "openChat";
        button2.setAttribute("class", "openChat");

        let myImg2 = document.createElement('img');
        const iconPath2 = $('#mapconversation').data('icon-open-chat');
        myImg2.src = iconPath2;
        myImg2.height = "16";
        myImg2.width = "16";

        button2.appendChild(myImg2);

        div1.appendChild(button2);


        let button1 = document.createElement('button');
        button1.title = "Supprimer le chat";
        //   button1.className = "deleteChat";
        button1.setAttribute("class", "deleteChat");

        let myImg1 = document.createElement('img');
        const iconPath1 = $('#mapconversation').data('icon-delete');
        myImg1.src = iconPath1;
        myImg1.height = "16";
        myImg1.width = "16";

        button1.appendChild(myImg1);

        div1.appendChild(button1);

        div0.appendChild(div1);

        grabList.appendChild(div0);
    }
}
function openChat() {
    alert("open Chat");
}
function deleteChat() {
    alert("delete Chat");
}