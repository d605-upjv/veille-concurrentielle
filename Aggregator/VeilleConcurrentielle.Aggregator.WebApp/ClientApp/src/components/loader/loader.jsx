import './loader.css';
import { CircularProgress } from '@mui/material';

export const Loader = () => {
    return (
        <div className="loader-container">
            <CircularProgress />
        </div>
    )
}