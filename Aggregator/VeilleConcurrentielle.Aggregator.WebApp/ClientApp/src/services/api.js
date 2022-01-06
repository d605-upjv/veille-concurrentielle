export async function getProducts() {
    const response = await fetch('/api/products');
    return await response.json();
}

export async function getProduct(productId) {
    const response = await fetch(`/api/products/${productId}`);
    return await response.json();
}

export async function getRecommendationAlertsCount() {
    const response = await fetch(`/api/recommendationalerts/count`);
    return await response.json();
}

export async function getRecommendationAlertsCountByProduct() {
    const response = await fetch(`/api/recommendationalerts/products/count`);
    return await response.json();
}

export async function getRecommendationAlerts() {
    const response = await fetch('/api/recommendationalerts');
    return await response.json();
}

export async function setRecommendationAlertsForProductToSeen(productId) {
    const response = await fetch(`/api/recommendationalerts/products/${productId}/seen`, {
        method: 'POST'
    });
    return await response.json();
}

export async function getMainShopProduct(productUrl) {
    const response = await fetch(`/api/mainshop?productUrl=${productUrl}`);
    return await response.json();
}

export async function GetProductToEdit(productId) {
    const response = await fetch(`/api/products/${productId}/edit`);
    return await response.json();
}

export async function GetProductToAdd() {
    const response = await fetch(`/api/products/add`);
    return await response.json();
}

export async function upsertProduct(product) {
    const response = await fetch('/api/products', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(product)
    });
    return await response.json();
}

export async function updateMainShopPrice(productId, price) {
    const response = await fetch('/api/mainshop/price', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({productId, price})
    });
    checkResponseStatus(response);
    return await response.json();
}

function checkResponseStatus(response) {
    if (!response.ok) {
        throw new Error(response.statusText);
    }
}