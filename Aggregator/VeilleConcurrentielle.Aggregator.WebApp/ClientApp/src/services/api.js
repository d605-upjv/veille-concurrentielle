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