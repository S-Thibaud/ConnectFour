window.addEventListener("load", init);
window.setInterval(getStatusHandler, 5000);
let jointrue = false;

function init() {
    let WaitingButton = document.getElementById('joinWaitingButton');
    WaitingButton.addEventListener("click", function (e) {
        e.preventDefault();
        if (WaitingButton.value === "Join waitingroom") {
            handleJoinWaiting();
        } else {
            WaitingButton.value = "Join waitingroom";
            handleLeaveWaiting();
        }
    });

    let statusButton = document.getElementById('getStatus');
    statusButton.addEventListener("click", getStatusInfoHandler);
}

function clearOutput(element) {
  while (element.firstChild) {
    element.removeChild(element.firstChild);
  }
}

function displayMessage(span, message) {
    span.textContent = message;
    span.style.fontSize = '16px';
}

function handleJoinWaiting() {
    jointrue = true;
    let meldingSpan = document.getElementById('melding');
    let rows = document.getElementById("gridRows").value;
    let columns = document.getElementById("gridColumns").value;
    let url = 'https://localhost:5001/api/WaitingPool/join/';
    let join =
        {
            "autoMatchCandidates": true,
            "enablePopOut": false,
            "connectionSize": 4,
            "gridRows": rows,
            "gridColumns": columns
        };

    document.getElementById('joinWaitingButton').disabled = true;

    fetch(url,
        {
            method: "POST",
            body: JSON.stringify(join),
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + sessionStorage.getItem("token")
            }
        })
        .then((response) => {
            if (response.status === 200) {
                displayMessage(meldingSpan, '<img src="assets/loading.webp" style="visibility: visible" alt="">');
                meldingSpan.innerHTML = 'Looking for opponents <img src="assets/loading.webp" style="visibility: visible" alt="">';
            } else {
                return response.json();
            }
        })
        .finally(() => {
            setTimeout(() => {
                let WaitingButton = document.getElementById('joinWaitingButton');
                WaitingButton.value = "Leave waitingroom";
                document.getElementById('joinWaitingButton').disabled = false;
            }, 0);
        })
}

function handleLeaveWaiting() {
    let meldingSpan = document.getElementById('melding');
    let url = 'https://localhost:5001/api/WaitingPool/leave/';
    displayMessage(meldingSpan, '');

    document.getElementById('joinWaitingButton').disabled = true;

    fetch(url,
        {
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + sessionStorage.getItem("token")
            }
        })
        .then((response) => {
            if (response.status === 200) {
                displayMessage(meldingSpan, 'You left the waitingpool');
            } else {
                return response.json();
            }
        })
        .finally(() => {
            setTimeout(() => {
                let WaitingButton = document.getElementById('joinWaitingButton');
                WaitingButton.value = "Join waitingroom";
                document.getElementById('joinWaitingButton').disabled = false;
            }, 0);
        })
    join = false;
}

function getStatusHandler() {
    if (jointrue === true) {
        let meldingSpan = document.getElementById('melding');
        let url = 'https://localhost:5001/api/WaitingPool/candidates/me/';
        displayMessage(meldingSpan, '');

        fetch(url,
            {
                method: "GET",
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + sessionStorage.getItem("token")
                }
            })
            .then((response) => {
                if (response.status === 200) {
                    return response.json();
                } else {
                    return response.json();
                }
            })
            .then((data) => {
                if (data.gameId !== "00000000-0000-0000-0000-000000000000") {
                    jointrue = false;
                    sessionStorage.setItem("gameid", data.gameId);
                    sessionStorage.setItem("userId", data.user.id);
                    window.open("game.html");
                    window.close();
                } else {
                    handleJoinWaiting();
                }
            })
            .finally(() => {
                setTimeout(() => {
                    //document.getElementById('getStatus').disabled = false;
                }, 1000);
            })
    }
}

function getStatusInfoHandler() {
    if (jointrue === true) {
        let url = 'https://localhost:5001/api/WaitingPool/candidates/me/';
        document.getElementById('getStatus').disabled = true;

        fetch(url,
            {
                method: "GET",
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + sessionStorage.getItem("token")
                }
            })
            .then((response) => {
                if (response.status === 200) {
                    return response.json();
                } else {
                    return response.json();
                }
            })
            .then((data) => {
                // Return game status
                const user = data.user;
                const gameSettings = data.gameSettings;
                const gameId = data.gameId;
                const proposedOpponentUserId = data.proposedOpponentUserId;

                const gameStatusOutput = document.getElementById("gameStatusOutput");
                clearOutput(gameStatusOutput);

                let statusHeader = document.createElement('h2');
                statusHeader.appendChild(document.createTextNode(`Game status for ${user.nickName}:`));
                gameStatusOutput.appendChild(statusHeader);

                let userParagraph = document.createElement('p');
                userParagraph.appendChild(document.createTextNode(`User ID: ${user.id}`));
                userParagraph.appendChild(document.createElement('br'));
                userParagraph.appendChild(document.createTextNode(`Email: ${user.email}`));
                userParagraph.appendChild(document.createElement('br'));
                userParagraph.appendChild(document.createTextNode(`Nickname: ${user.nickName}`));
                gameStatusOutput.appendChild(userParagraph);

                let gameSettingsParagraph = document.createElement('p');
                gameSettingsParagraph.appendChild(document.createTextNode(`Auto Match Candidates: ${gameSettings.autoMatchCandidates}`));
                gameSettingsParagraph.appendChild(document.createElement('br'));
                gameSettingsParagraph.appendChild(document.createTextNode(`Enable Pop Out: ${gameSettings.enablePopOut}`));
                gameSettingsParagraph.appendChild(document.createElement('br'));
                gameSettingsParagraph.appendChild(document.createTextNode(`Connection Size: ${gameSettings.connectionSize}`));
                gameSettingsParagraph.appendChild(document.createElement('br'));
                gameSettingsParagraph.appendChild(document.createTextNode(`Grid Rows: ${gameSettings.gridRows}`));
                gameSettingsParagraph.appendChild(document.createElement('br'));
                gameSettingsParagraph.appendChild(document.createTextNode(`Grid Columns: ${gameSettings.gridColumns}`));
                gameSettingsParagraph.appendChild(document.createElement('br'));
                gameStatusOutput.appendChild(gameSettingsParagraph);

                let gameIdParagraph = document.createElement('p');
                gameIdParagraph.appendChild(document.createTextNode(`Your Game ID: ${gameId}`));
                gameStatusOutput.appendChild(gameIdParagraph);

                let proposedOpponentUserIdParagraph = document.createElement('p');
                proposedOpponentUserIdParagraph.appendChild(document.createTextNode(`Opponent User ID: ${proposedOpponentUserId}`));
                gameStatusOutput.appendChild(proposedOpponentUserIdParagraph);

                // Starting a game
                if (data.gameId !== "00000000-0000-0000-0000-000000000000") {
                    jointrue = false;
                    sessionStorage.setItem("gameid", data.gameId);
                    window.open("game.html");
                    window.close();
                } else {
                    handleJoinWaiting();
                }
            })
            .finally(() => {
                setTimeout(() => {
                    document.getElementById('getStatus').disabled = false;
                }, 1000);
            })
    }
}