const { performance } = require("perf_hooks")

let str = "CE CERE SOWECHEHE AHOUND VAHSTOC ON THE EDYE OB THE DESEHT CHEN THE DHUYS VEYAN TO TAJE HOLDF I HEWEWVEH SAGINY SOWETHINY LIJE XI BEEL A VIT LIYHTHEADED; WAGVE GOU SHOULD DHIZEF F F FX AND SUDDENLG THEHE CAS A TEHHIVLE HOAH ALL AHOUND US AND THE SJG CAS BULL OB CHAT LOOJED LIJE HUYE VATSQ ALL SCOOKINY AND SMHEEMHINY AND DIZINY AHOUND THE MAHQ CHIMH CAS YOINY AVOUT A HUNDHED WILES AN HOUH CITH THE TOK DOCN TO LAS ZEYASF"

let map = new Map([
    ["C", "W"], 
    ["W", "M"],
    ["H", "R"],
    ["B", "F"],
    ["Y", "G"],
]) 

const y = str.split("").map(letter => {
    const mapped = map.get(letter)
    if (mapped) return mapped
    else return letter
}).join("")


console.log(y)
