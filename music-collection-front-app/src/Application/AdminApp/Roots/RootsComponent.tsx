import React, {useEffect, useState} from "react";
import {RootDto} from "../Dto/RootDto";
import FilesApiClient from "../Api/Users/FilesApiClient";
import "./RootsComponent.css"
import RootsListElement from "./RootsListElement";
import {Button, Dialog, DialogActions, DialogContent, DialogTitle, IconButton, TextField} from "@mui/material";
import CloseIcon from '@mui/icons-material/Close';

export default function RootsComponent(): React.ReactElement {
    const [rootsList, setRoots] = useState<RootDto[]>([]);
    const [showCreateRootModal, setShowCreateRootModal] = useState(false);
    const [newRootName, setNewRootName] = useState("");
    const [newRootPath, setNewRootPath] = useState("");
    const [newRootNameError, setNewRootNameError] = useState(false)
    const [newRootPathError, setNewRootPathError] = useState(false)

    async function updateList(): Promise<void> {
        const roots = await FilesApiClient.getAllRoots()
        setRoots(roots)
    }

    function closeCreateRootModal() {
        setNewRootName("")
        setNewRootPath("")
        setShowCreateRootModal(false);
    }

    useEffect(() => {
        updateList().catch(console.error)
    }, []);

    return (
        <div>
            <div className="rootsHeader">
                <b className="rootsTitle">Roots</b>
                <div className="rootsCreateButton" onClick={() => setShowCreateRootModal(true)}>Create root</div>
            </div>
            <table className="rootsTable">
                <tr>
                    <th>Id</th>
                    <th>Name</th>
                    <th>Path</th>
                </tr>
                {
                    rootsList.map(x => <RootsListElement root={x}/>)
                }
            </table>
            <Dialog open={showCreateRootModal} onClose={closeCreateRootModal}>
                <DialogTitle>
                    Create new root
                    <IconButton
                        aria-label="close"
                        onClick={closeCreateRootModal}
                        sx={{
                            position: 'absolute',
                            right: 8,
                            top: 8,
                            color: (theme) => theme.palette.grey[500],
                        }}
                    >
                        <CloseIcon/>
                    </IconButton>
                </DialogTitle>
                <DialogContent>
                    <TextField
                        error={newRootNameError}
                        autoFocus
                        margin="dense"
                        id="name"
                        label="Name"
                        fullWidth
                        value={newRootName}
                        onChange={x => setNewRootName(x.target.value)}
                    />
                    <TextField
                        error={newRootPathError}
                        margin="dense"
                        id="path"
                        label="Path"
                        fullWidth
                        value={newRootPath}
                        onChange={x => setNewRootPath(x.target.value)}
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={closeCreateRootModal}>Cancel</Button>
                    <Button onClick={async () => {
                        setNewRootNameError(newRootName.length === 0);
                        setNewRootPathError(newRootPath.length === 0);
                        if (newRootNameError || newRootPathError) {
                            return;
                        }
                        await FilesApiClient.createRoot(newRootName, newRootPath);
                        closeCreateRootModal();
                        await updateList();
                    }}>Create</Button>
                </DialogActions>
            </Dialog>
        </div>
    );
}