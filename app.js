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
    setThree(true);
    setThree(true);
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

function setThree(movement) {
    if (full()) {
        return;
    }
    if (!movement) {
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
        let m = slideLeft();
        document.getElementsByClassName("score")[0].innerHTML = score.toString();
        setThree(m);
    } else if (e.code == "ArrowRight") {
        let m = slideRight();
        document.getElementsByClassName("score")[0].innerHTML = score.toString();
        setThree(m);
    } else if (e.code == "ArrowUp") {
        let m = slideUp();
        document.getElementsByClassName("score")[0].innerHTML = score.toString();
        setThree(m);
    } else if (e.code == "ArrowDown") {
        let m = slideDown();
        document.getElementsByClassName("score")[0].innerHTML = score.toString();
        setThree(m);
    }
})

function removeZerosLeftUp(arr) {
    let changes = false;
    let nonZeros = [];
    let length = arr.length;
    for (let i = 0; i < length; i++) {
        if (arr[i] !== 0) {
            nonZeros.push(arr[i]);
        }
    }
    let len = nonZeros.length;
    for (let i = 0; i < length; i++) {
        if (i >= len) {
            if (arr[i] !== 0) {
                changes = true;
            }
            arr[i] = 0;
        } else {
            if (arr[i] !== nonZeros[i]) {
                changes = true;
            }
            arr[i] = nonZeros[i];
        }
    }
    return changes;
}

function slideLeft() {
    let movement = false;
    for (let r = 0; r < rows; r++) {
        movement = removeZerosLeftUp(board[r]) || movement;
        for (let c = 0; c < columns - 1; c++) {
            if (board[r][c] == board[r][c + 1]) {
                movement = true;
                score += board[r][c] * 3;
                board[r][c] *= 3;
                board[r][c + 1] = 0;
                c++;
            }
        }
        removeZerosLeftUp(board[r]);
        for (let c = 0; c < columns; c++) {
            let tile = document.getElementById(r.toString() + "-" + c.toString());
            updateTile(tile, board[r][c]);
        }
    }
    return movement;
}

function removeZerosRightDown(arr) {
    let changes = false;
    let nonZeros = [];
    let length = arr.length;
    for (let i = 0; i < length; i++) {
        if (arr[i] !== 0) {
            nonZeros.push(arr[i]);
        }
    }
    let len = length - nonZeros.length;
    let index = 0;
    for (let i = 0; i < length; i++) {
        if (i < len) {
            if (arr[i] !== 0) {
                changes = true;
            }
            arr[i] = 0;
        } else {
            if (arr[i] !== nonZeros[index]) {
                changes = true;
            }
            arr[i] = nonZeros[index];
            index++;
        }
    }
    return changes;
}

function slideRight() {
    let movement = false;
    for (let r = 0; r < rows; r++) {
        movement = removeZerosRightDown(board[r]) || movement;
        for (let c = columns - 1; c > 0; c--) {
            if (board[r][c] == board[r][c - 1]) {
                movement = true;
                score += board[r][c] * 3;
                board[r][c] *= 3;
                board[r][c - 1] = 0;
                c--;
            }
        }
        removeZerosRightDown(board[r]);
        for (let c = 0; c < columns; c++) {
            let tile = document.getElementById(r.toString() + "-" + c.toString());
            updateTile(tile, board[r][c]);
        }
    }
    return movement;
}

function slideUp() {
    let movement = false;
    for (let c = 0; c < columns; c++) {
        let colArr = [];
        for (let r = 0; r < rows; r++) {
            colArr.push(board[r][c]);
        }
        movement = removeZerosLeftUp(colArr) || movement;
        for (let r = 0; r < rows - 1; r++) {
            if (colArr[r] == colArr[r + 1]) {
                movement = true;
                score += colArr[r] * 3;
                colArr[r] *= 3;
                colArr[r + 1] = 0;
                r++;
            }
        }
        removeZerosLeftUp(colArr);
        for (let r = 0; r < rows; r++) {
            board[r][c] = colArr[r];
            let tile = document.getElementById(r.toString() + "-" + c.toString());
            updateTile(tile, board[r][c]);
        }
    }
    return movement;
}

function slideDown() {
    let movement = false;
    for (let c = 0; c < columns; c++) {
        let colArr = [];
        for (let r = 0; r < rows; r++) {
            colArr.push(board[r][c]);
        }
        movement = removeZerosRightDown(colArr) || movement;
        for (let r = rows - 1; r > 0; r--) {
            if (colArr[r] == colArr[r - 1]) {
                movement = true;
                score += colArr[r] * 3;
                colArr[r] *= 3;
                colArr[r - 1] = 0;
                r--;
            }
        }
        removeZerosRightDown(colArr);
        for (let r = 0; r < rows; r++) {
            board[r][c] = colArr[r];
            let tile = document.getElementById(r.toString() + "-" + c.toString());
            updateTile(tile, board[r][c]);
        }
    }
    return movement;
}