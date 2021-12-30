export async function getProducts() {
    const response = await fetch('/api/products');
    return await response.json();
}

export async function getProduct(productId) {
    const response = await fetch(`/api/products/${productId}`);
    return await response.json();
}