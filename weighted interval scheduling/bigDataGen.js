const fs = require("fs")

const arr = []
arr.push("10000")
for (let i = 0; i < 10000; i++) {
    arr.push("1000000000 2000000000 1000000000")
}

const s = arr.join("\n")

fs.writeFileSync("./10k.txt", s)