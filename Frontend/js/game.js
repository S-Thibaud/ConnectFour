window.addEventListener("load", init);
let numberOfRows = 0;
let numberOfColumns = 0;

function init() {
    retrieveGameInfo();
    retrieveplayersInfo();

    let statusButton = document.getElementById('getStatus');
    statusButton.addEventListener("click", function (event) {
        event.preventDefault();
        getStatusInfoHandler();
    });
}

// Call handleGameId every two seconds
let timer = setInterval(retrieveGameInfo, 2000);

function clearOutput(element) {
    while (element.firstChild) {
        element.removeChild(element.firstChild);
    }
}

function retrieveplayersInfo() {
    let url = `https://localhost:5001/api/Games/${sessionStorage.getItem("gameid")}/`;
    let finished = document.getElementById('showfinished');
    finished.style.visibility = 'hidden';
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
            return response.json();

        })
        .then((data) => {
            let left = document.getElementById("playerleft");
            let right = document.getElementById("playerright");

            // linker speler invoegen
            let playerImageLeft = document.createElement('img');
            if (data.player1.color === 1) {
                playerImageLeft.setAttribute('src', 'assets/red.png');
            } else {
                playerImageLeft.setAttribute('src', 'assets/yellow.png');
            }
            playerImageLeft.setAttribute('alt', 'Player 1');
            left.appendChild(playerImageLeft);
            if (sessionStorage.getItem("userId") === data.player1.id) {
                left.appendChild(document.createTextNode(data.player1.name + " (You)"));
            } else {
                left.appendChild(document.createTextNode(data.player1.name));
            }

            // rechter speler invoegen
            let playerImageRight = document.createElement('img');
            if (data.player2.color === 2) {
                playerImageRight.setAttribute('src', 'assets/yellow.png');
            } else {
                playerImageRight.setAttribute('src', 'assets/red.png');
            }
            playerImageRight.setAttribute('alt', 'Player 2');
            right.appendChild(playerImageRight);
            if (sessionStorage.getItem("userId") === data.player2.id) {
                right.appendChild(document.createTextNode(data.player2.name + " (You)"));
            } else {
                right.appendChild(document.createTextNode(data.player2.name));
            }
        })
}

function retrieveGameInfo() {
    let url = `https://localhost:5001/api/Games/${sessionStorage.getItem("gameid")}/`;
    let finished = document.getElementById('showfinished');
    finished.style.visibility = 'hidden';
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
            return response.json();

        })
        .then((data) => {
            maketable(data)

            //console.log(data);
            if (data.finished === true) {
                //console.log("GAME IS FINISHED");
                let winner = document.createElement('h1');
                let winningcolor;
                if (data.grid.winningConnections.length === 0) {
                    // let yourTurnSpanLeft = document.getElementById('yourTurnSpanLeft');
                    // let yourTurnSpanRight = document.getElementById('yourTurnSpanRight');
                    // yourTurnSpanLeft.style.visibility = "hidden";
                    // yourTurnSpanRight.style.visibility = "hidden";
                    // yourTurnSpanLeft.removeChild(yourTurnSpanLeft.firstChild);
                    // yourTurnSpanRight.removeChild(yourTurnSpanLeft.firstChild);


                    winningcolor = document.createTextNode("gelijk spel!");

                    finished.appendChild(winningcolor);

                    finished.style.visibility = 'visible';
                    clearInterval(timer);
                } else {
                    if (data.grid.winningConnections[0].color === 1) {
                        winningcolor = document.createTextNode("Rood heeft gewonnen!");
                    } else {
                        winningcolor = document.createTextNode("Geel heeft gewonnen!");
                    }

                    // const currentPlayerId = data.playerToPlayId;
                    // const player1Id = data.player1.id;
                    // const player2Id = data.player2.id;

                    // if (currentPlayerId === sessionStorage.getItem('userId')) {
                    //     winningcolor = document.createTextNode("U heeft gewonnen!");
                    // } else {
                    //     winningcolor = document.createTextNode("U heeft verloren.");
                    // }
                    // } else {
                    //     winningcolor = document.createTextNode("Spel afgelopen.");
                    // }


                    finished.appendChild(winningcolor);

                    //Winning connection aanduiden
                    let fromrow = data.grid.winningConnections[0].from.row;
                    let fromcolumn = data.grid.winningConnections[0].to.column;
                    let torow = data.grid.winningConnections[0].from.row;
                    let tocolumn = data.grid.winningConnections[0].to.column;


                    finished.style.visibility = 'visible';
                    clearInterval(timer);
                    // Add the winning effect to the body element
                    document.body.classList.add('winning-effect');


                    // Generate confetti elements dynamically
                    for (let i = 0; i < 100; i++) {
                        const confetti = document.createElement('div');
                        confetti.classList.add('confetti');
                        confetti.style.left = `${Math.random() * 100}%`;
                        confetti.style.animationDelay = `${Math.random() * 3}s`;
                        document.body.appendChild(confetti);
                    }
                }

                // break element zodat de button op de volgende lijn komt
                let breakelement = document.createElement("br");
                finished.appendChild(breakelement);
                // knop om naar de waitingroom te gaan
                let returnbutton = document.createElement("button");
                returnbutton.id = "returnbutton";
                returnbutton.textContent = "terug naar de waitingroom";
                returnbutton.addEventListener('click', () => {
                    window.open("waitingroom.html");
                    window.close();
                });
                finished.appendChild(returnbutton);


                let tablehead = document.getElementsByTagName("thead");
                tablehead[0].style.display = "none";
            } else {
                //console.log("en toch ben ik HIER");
                const buttons = document.getElementsByTagName('button');
                //console.log("test length buttons: ", buttons.length);
                for (let i = 0; i < buttons.length; i++) {
                    //console.log("test");
                    let button = buttons[i];
                    button.addEventListener('click', () => {
                        handleMove(sessionStorage.getItem("gameid"), button.id);
                        //console.log('werkt');
                    });
                }
            }

            let yourTurnSpanLeft = document.getElementById('yourTurnSpanLeft');
            let yourTurnSpanRight = document.getElementById('yourTurnSpanRight');


            if (sessionStorage.getItem("userId") === data.playerToPlayId && sessionStorage.getItem("userId") === data.player1.id) {
                yourTurnSpanLeft.style.visibility = "visible";
            } else {
                yourTurnSpanLeft.style.visibility = "hidden";
            }

            if (sessionStorage.getItem("userId") === data.playerToPlayId && sessionStorage.getItem("userId") === data.player2.id) {
                yourTurnSpanRight.style.visibility = "visible";
            } else {
                yourTurnSpanRight.style.visibility = "hidden";
            }


        })
        .then(() => {
            handlePossibleMoves(sessionStorage.getItem("gameid"));
        })
}

function maketable(data) {
    numberOfRows = data.grid.numberOfRows;
    numberOfColumns = data.grid.numberOfColumns;
    let cells = data.grid.cells;
    //console.log("cells zijn: ", cells);
    let highest = 0;

    if (numberOfRows > numberOfColumns) {
        highest = numberOfRows
    } else {
        highest = numberOfColumns;
    }

    let table = document.createElement("table");
    let tablehead = document.createElement("thead");
    let tablebody = document.createElement("tbody");
    let tablerow = document.createElement("tr");
    let output = document.getElementById("spelbord");
    clearOutput(output);
    let tablecol;


    for (let row = 0; row < highest; row++) {
        tablebuttonhead = document.createElement("button");
        tablebuttonhead.setAttribute('class', 'slideInButton');
        tablebuttonhead.style.visibility = "hidden";
        tablebuttonhead.id = row.toString();
        tablecolhead = document.createElement("td");
        tablecolhead.appendChild(tablebuttonhead);
        tablerow.appendChild(tablecolhead);
    }

    tablehead.appendChild(tablerow);

    if (data.finished === true && data.grid.winningConnections.length > 0) {
        //Winning connection aanduiden
        let fromrow = data.grid.winningConnections[0].from.row;
        let fromcolumn = data.grid.winningConnections[0].from.column;
        let torow = data.grid.winningConnections[0].to.row;
        let tocolumn = data.grid.winningConnections[0].to.column;
        console.log(fromrow);
        console.log(fromcolumn);
        console.log(torow);
        console.log(tocolumn);

        for (let row = 0; row < numberOfRows; row++) {
            tablerow = document.createElement("tr");
            tablebody.appendChild(tablerow);

            for (let col = 0; col < numberOfColumns; col++) {
                tablecol = document.createElement("td");

                if (cells[row][col] != null) {


                    if (cells[row][col].Color === 1) {
                        tablecol.style.backgroundColor = "red";
                    } else if (cells[row][col].Color === 2) {

                        tablecol.style.backgroundColor = "yellow";
                    } else {
                        tablecol.appendChild(document.createTextNode("leeg"));
                    }
                    if (fromrow === torow) {
                        for (let teller = fromcolumn; teller <= tocolumn; teller++) {
                            if (col === teller && row === fromrow) {
                                tablecol.style.backgroundColor = "green";
                            }
                        }
                    } else if (fromcolumn === tocolumn) {
                        for (let teller = fromrow; teller <= torow; teller++) {
                            if (row === teller && col === fromcolumn) {
                                tablecol.style.backgroundColor = "green";
                            }
                        }
                    } else if (fromrow > torow) {
                        let counter = 1;
                        for (let teller = fromrow; teller >= torow; teller--) {
                            if (row === teller - 1 && col - (counter * 1) === fromcolumn || fromcolumn === col && fromrow === row) {
                                tablecol.style.backgroundColor = "green";
                            }
                            counter++;
                        }
                    } else {
                        let counter = torow - fromrow;
                        for (let teller = fromrow; teller <= torow; teller++) {
                            if (row === teller && col + (counter * 1) === tocolumn) {
                                tablecol.style.backgroundColor = "green";
                            }
                            counter--;
                        }
                    }
                }

                tablerow.appendChild(tablecol);
            }
        }
    } else {
        // table body
        for (let row = 0; row < numberOfRows; row++) {
            tablerow = document.createElement("tr");
            tablebody.appendChild(tablerow);

            for (let col = 0; col < numberOfColumns; col++) {
                tablecol = document.createElement("td");
                if (cells[row][col] != null) {
                    if (cells[row][col].Color === 1) {
                        tablecol.style.backgroundColor = "red";
                    } else if (cells[row][col].Color === 2) {
                        tablecol.style.backgroundColor = "yellow";
                    } else {
                        tablecol.appendChild(document.createTextNode("leeg"));
                    }
                }
                tablerow.appendChild(tablecol);
            }
        }
    }

    table.appendChild(tablehead);
    table.appendChild(tablebody);
    output.appendChild(table);
}

function handleMove(id, buttonid) {
    let url = `https://localhost:5001/api/Games/${id}/move`;
    let body =
        {
            "type": 1,
            "discType": 1,
            "column": buttonid
        }

    fetch(url,
        {
            method: "POST",
            body: JSON.stringify(body),
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + sessionStorage.getItem("token")
            }
        })
        .then((response) => {
            if (response.status === 200) {
                // game info opvragen + tabel hernieuwen
                retrieveGameInfo();
            } else {
                return response.json();
            }
        })

}

function handlePossibleMoves(id) {
    let url = `https://localhost:5001/api/Games/${id}/possible-moves/`;

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
            data.forEach(element => {
                let elements = document.getElementById(element.column);
                elements.style.visibility = "visible";
            });
        })
}

function getStatusInfoHandler() {
    let url = `https://localhost:5001/api/Games/${sessionStorage.getItem("gameid")}/`;
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
            const gameStatusOutput = document.getElementById('gameStatusOutput');
            clearOutput(gameStatusOutput);

            let statusHeader = document.createElement('h2');
            statusHeader.appendChild(document.createTextNode(`Detailed game information:`));
            gameStatusOutput.appendChild(statusHeader);

            let idParagraph = document.createElement('p');
            idParagraph.appendChild(document.createTextNode(`ID: ${data.id}`));
            gameStatusOutput.appendChild(idParagraph);

            let player1Paragraph = document.createElement('p');
            if (sessionStorage.getItem("userId") === data.player1.id) {
                player1Paragraph.appendChild(document.createTextNode('Player 1: (You)'));
            } else {
                player1Paragraph.appendChild(document.createTextNode('Player 1:'));
            }
            player1Paragraph.appendChild(document.createElement('br'));
            player1Paragraph.appendChild(document.createTextNode(`ID - ${data.player1.id}`));
            player1Paragraph.appendChild(document.createElement('br'));
            player1Paragraph.appendChild(document.createTextNode(`Name - ${data.player1.name}`));
            player1Paragraph.appendChild(document.createElement('br'));
            player1Paragraph.appendChild(document.createTextNode(`Color - ${data.player1.color}`));
            player1Paragraph.appendChild(document.createElement('br'));
            player1Paragraph.appendChild(document.createTextNode(`Number of Normal Discs - ${data.player1.numberOfNormalDiscs}`));
            gameStatusOutput.appendChild(player1Paragraph);

            let player2Paragraph = document.createElement('p');
            if (sessionStorage.getItem("userId") === data.player2.id) {
                player2Paragraph.appendChild(document.createTextNode('Player 2: (You)'));
            } else {
                player2Paragraph.appendChild(document.createTextNode('Player 2:'));
            }
            player2Paragraph.appendChild(document.createElement('br'));
            player2Paragraph.appendChild(document.createTextNode(`ID - ${data.player2.id}`));
            player2Paragraph.appendChild(document.createElement('br'));
            player2Paragraph.appendChild(document.createTextNode(`Name - ${data.player2.name}`));
            player2Paragraph.appendChild(document.createElement('br'));
            player2Paragraph.appendChild(document.createTextNode(`Color - ${data.player2.color}`));
            player2Paragraph.appendChild(document.createElement('br'));
            player2Paragraph.appendChild(document.createTextNode(`Number of Normal Discs - ${data.player2.numberOfNormalDiscs}`));
            gameStatusOutput.appendChild(player2Paragraph);

            let playerToPlayIdParagraph = document.createElement('p');
            playerToPlayIdParagraph.appendChild(document.createTextNode(`Player to Play ID: ${data.playerToPlayId}`));
            gameStatusOutput.appendChild(playerToPlayIdParagraph);

            let gridParagraph = document.createElement('p');
            gridParagraph.appendChild(document.createTextNode('Grid:'));
            gridParagraph.appendChild(document.createElement('br'));
            gridParagraph.appendChild(document.createTextNode(`Number of Rows - ${data.grid.numberOfRows}`));
            gridParagraph.appendChild(document.createElement('br'));
            gridParagraph.appendChild(document.createTextNode(`Number of Columns - ${data.grid.numberOfColumns}`));
            gridParagraph.appendChild(document.createElement('br'));
            gridParagraph.appendChild(document.createTextNode(`Winning Connect Size - ${data.grid.winningConnectSize}`));
            gameStatusOutput.appendChild(gridParagraph);

            let finishedParagraph = document.createElement('p');
            finishedParagraph.appendChild(document.createTextNode(`Finished: ${data.finished}`));
            gameStatusOutput.appendChild(finishedParagraph);

            let popOutAllowedParagraph = document.createElement('p');
            popOutAllowedParagraph.appendChild(document.createTextNode(`Pop Out Allowed: ${data.popOutAllowed}`));
            gameStatusOutput.appendChild(popOutAllowedParagraph);
        })
        .finally(() => {
            setTimeout(() => {
                document.getElementById('getStatus').disabled = false;
            }, 1000);
        })
}