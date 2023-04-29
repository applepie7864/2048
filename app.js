var board;
var score = 0;
var rows = 4;
var columns = 4;

window.onload = function() {
    setGame();
}

function setGame() {
    board = [[0, 0, 0, 0],
            [0, 0, 0, 0],
            [0, 0, 0, 0],
            [0, 0, 0, 0]];
    for (let r = 0; r < rows; r++) {
        for (let c = 0; c < columns; c++) {
            let tile = document.createElement("div");
            tile.id = r.toString() + "-" + c.toString();
            let num = board[r][c];
            updateTile(tile, num);
            document.getElementsByClassName("board")[0].appendChild(tile);
        }
    }
    setThree();
    setThree();
}

function full() {
    for (let r = 0; r < rows; r++) {
        for (let c = 0; c < columns; c++) {
            if (board[r][c] == 0) {
                return false;
            }
        }
    }
    return true;
}

function setThree() {
    if (full()) {
        return;
    }
    let found = false;
    while (!found) {
        let r = Math.floor(Math.random() * rows);
        let c = Math.floor(Math.random() * columns);
        if (board[r][c] == 0) {
            board[r][c] = 3;
            let tile = document.getElementById(r.toString() + "-" + c.toString());
            updateTile(tile, 3);
            found = true;
        }
    }
}

function updateTile(tile, num) {
    tile.innerText = "";
    tile.classList.value = "";
    tile.classList.add("tile");
    if (num > 0) {
        tile.innerText = num;
        if (num <= 2187) {
            tile.classList.add("t" + num.toString());
        }
    }
}

document.addEventListener("keyup", (e) => {
    if (e.code == "ArrowLeft") {
        slideLeft();
        setThree();
    } else if (e.code == "ArrowRight") {
        slideRight();
        setThree();
    } else if (e.code == "ArrowUp") {
        slideUp();
        setThree();
    } else if (e.code == "ArrowDown") {
        slideDown();
        setThree();
    }
})

function removeZeros(arr) {
    let nonZeros = []
    for (let i = 0; i < columns; i++) {
        if (arr[i] !== 0) {
            nonZeros.push(arr[i]);
        }
    }
    let len = nonZeros.length;
    for (let i = 0; i < columns; i++) {
        if (i >= len) {
            arr[i] = 0;
        } else {
            arr[i] = nonZeros[i];
        }
    }
    return arr;
}

function slideLeft() {
    for (let r = 0; r < rows; r++) {
        removeZeros(board[r]);
        for (let c = 0; c < columns - 1; c++) {
            if (board[r][c] == board[r][c + 1]) {
                board[r][c] *= 3;
                board[r][c + 1] = 0;
                c++;
            }
        }
        removeZeros(board[r]);
        for (let c = 0; c < columns; c++) {
            let tile = document.getElementById(r.toString() + "-" + c.toString());
            updateTile(tile, board[r][c]);
        }
    }
}

function slideRight() {

}

function slideUp() {

}

function slideDown() {

}