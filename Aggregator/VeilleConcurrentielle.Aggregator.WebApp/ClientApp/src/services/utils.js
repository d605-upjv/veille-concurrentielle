export function getDatetimeToDisplay(serverDate) {
    return (new Date(serverDate)).toLocaleString("fr");
}